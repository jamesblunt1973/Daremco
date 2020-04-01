$(document).ready(function () {
  setupRateing();
  setupIcons();
  setupMaterialsChart();
  setupDownloadPlan();
  setupPictureSlideshow();
  setupShopping();
  setupBulkTypes();
  setupMainImage();
});

function setupRateing() {
  var rateIt = $('.rateit');
  rateIt.rateit();
  rateIt.bind('rated', function () {
    var productId = +this.dataset.productId;
    var value = $(this).rateit('value');
    var data = JSON.stringify({
      productId: productId,
      rate: value
    });
    $.ajax({
      type: 'POST',
      url: productApi + 'rate',
      data: data,
      contentType: "application/json; charset=utf-8",
      dataType: "json"
    }).done(function (res) {
      var round = Math.round(res.rate * 100) / 100;
      rateIt.rateit('value', round);
      rateIt.next().text(round + ' (' + res.votes + ' رای)');
      rateIt.rateit('readonly', true);
    }).fail(function (request, status, error) {
      var panel = ajaxError(request, status, error);
      showPanel(panel, true, false);
    });
  });
}

function setupIcons() {
  $('[data-desc]').click(function () {
    var str = this.dataset.desc;
    var panel = createPanel(str, false, '', '', 480);
    showPanel(panel, false, true);
  });
}

function setupMaterialsChart() {
  $('.skin-materials').click(function () {
    var id = this.dataset.id;
    var totalTies = this.dataset.ties;
    var panel = new showPanel(getLoading(), false, false);
    $.get(productApi + 'productcolorgroups/' + id).done(function (colorGroups) {
      // نمایش تعداد رنگ و درصد هر جنس
      var tbl = $('<table />').attr({ 'cellpadding': '0', 'cellspacing': '0', 'border': '1', 'class': 'table-list' });
      tbl.append('<tr><th>نام جنس</th><th>تعداد رنگ</th><th>درصد مصرف</th></tr>');

      var chartPanel = $('<div />').addClass('chart-panel');
      var canvas = $('<canvas />').attr({ 'width': '300px', 'height': '300px' });
      chartPanel.append(canvas);
      var pieColors = [
        { color: '#F7464A', highlight: '#FF5A5E' },
        { color: '#46BFBD', highlight: '#5AD3D1' },
        { color: '#FDB45C', highlight: '#FFC870' }
      ];
      var data = [];
      for (i in colorGroups) {
        colorGroup = colorGroups[i];
        var percent = Math.round(colorGroup.tiesCount / totalTies * 10000) / 100;

        var tr = $('<tr />').css('background-color', pieColors[i].color);
        tr.append($('<td />').text(colorGroup.materialName));
        tr.append($('<td />').attr('align', 'center').text(colorGroup.colorsCount));
        tr.append($('<td />').attr('align', 'center').text('%' + percent));
        tbl.append(tr);

        var label = colorGroup.materialName + ' - ' + colorGroup.colorsCount + ' رنگ';
        //list.push(color.count / totalTies * 100);
        var obj = {};
        obj.value = percent;
        obj.color = pieColors[i].color;
        obj.highlight = pieColors[i].highlight;
        obj.label = label;
        data.push(obj);
      }
      var ctx = canvas.get(0).getContext('2d');
      new Chart(ctx).Pie(data);

      chartPanel.append(tbl);

      var content = createPanel(chartPanel, true, 'جدول و نمودار توزیع جنس', 'dkb-chart-pie');
      panel.setContent(content);
      panel.setClosable(true, false);
    }).fail(function (request, status, error) {
      panel.setContent(ajaxError(request, status, error));
      panel.setClosable(true, false);
    });
  });
  $('.bulk-materials').click(function () {
    var id = this.dataset.id;
    var panel = new showPanel(getLoading(), false, false);
    $.get(productApi + 'productcolors/' + id).done(function (materials) {
      var tbl = $('<table />').attr({ 'cellpadding': '0', 'cellspacing': '0', 'border': '1', 'class': 'table-list' });
      tbl.append('<tr><th>شماره</th><th>رنگ</th><th>شماره در پالت</th><th>وزن دقیق</th><th>وزن بسته</th><th>جنس</th><th>قیمت</th></tr>');
      for (var i in materials) {
        var material = materials[i];
        var tr = $('<tr />');

        var td = $('<td />').attr('align', 'center').text(eval(i) + 1);
        tr.append(td);

        td = $('<td />').css('background-color', material.color);
        tr.append(td);

        td = $('<td />').attr('align', 'center').text(material.position);
        tr.append(td);

        td = $('<td />').attr('align', 'center').text(material.weight);
        tr.append(td);

        td = $('<td />').attr('align', 'center').text(material.bulkWeight);
        tr.append(td);

        td = $('<td />').text(material.material);
        tr.append(td);

        td = $('<td />').attr('align', 'left').text(formatCurrency(material.price));
        tr.append(td);

        tbl.append(tr);
      }
      var content = createPanel(tbl, true, 'پالت رنگ و جنس محصول', 'dkb-view-list');
      panel.setContent(content);
      panel.setClosable(true, false);
    }).fail(function (request, status, error) {
      panel.setContent(ajaxError(request, status, error));
      panel.setClosable(true, false);
    });
  });
}

function setupDownloadPlan() {
  var panels = ['', 'numerical-plan-panel', 'cross-plan-panel', 'audio-plan-panel'];
  $('.plan-download-btn').click(function () {
    var planTypeId = this.dataset.planTypeId;
    var planId = this.dataset.planId;
    var panelId = panels[planTypeId];

    var panelContent = $('#' + panelId).clone(true);
    var content = createPanel(panelContent, true, 'دانلود نقشه', 'dkb-download');
    var panel = new showPanel(content, true, false);
    panelContent.show();

    content.find('input[name="planId"]').val(planId);
    content.find('button').click(function () {
      panel.close();
    });
  });
  $('#audio-plan-panel input[name="joola"]').change(function () {
    $(this).parents('form').children('.serial').slideToggle();
  });
}

function setupPictureSlideshow() {
  $('.image[data-slides]').click(function () {
    var slides = this.dataset.slides.split(',');
    var path = this.dataset.path;

    var slidesPanel = $('<div />').addClass('slide-panel');
    var scene = $('<div />').addClass('slide-image');
    var img = $('<img />').attr({ 'src': path + slides[0] });
    scene.append(img);

    var thumbs = $('<div />').addClass('slide-thumbs');

    for (var i in slides) {
      var thumb = $('<img />').attr('src', path + '_' + slides[i]);
      thumb.click({ index: i }, function (e) {
        thumbs.children().removeClass('selected');
        $(this).addClass('selected');
        img.attr({ 'src': path + slides[e.data.index] });
      });
      thumbs.append(thumb);
    }

    slidesPanel.append(scene, thumbs);
    thumbs.children().first().addClass('selected');

    var panel = createPanel(slidesPanel, true);
    showPanel(panel, true, false);
  });
}

function setupShopping() {
  $('.shop-btn').click(function () {
    // get cookie for user name and password
    var id = +this.dataset.id;
    var entity = this.dataset.entity;
    var un = $.cookie("un");
    var pw = $.cookie("pw");
    if (!un || !pw) {
      // show login form
      var panelContent = $('#login-panel').clone(true);
      var content = createPanel(panelContent, true, 'ورود به سایت', 'dkb-customer');
      var panel = new showPanel(content, true, false);
      //content.find('.reset').click(function () {
      //  panel.close();
      //});
      content.find('.apply').click(function () {
        content.find('.result').hide();
        var form = content.find('form');
        if (!form.is(':valid')) {
          content.find('.error-msg').text('لطفا شماره همراه معتبر و کلمه عبور را وارد کنید').show();
          return;
        }
        var cell = content.find('input[type="text"]').val();
        var pass = content.find('input[type="password"]').val();
        var save = content.find('input[type="checkbox"]').is(':checked');

        var data = JSON.stringify({
          id: id,
          entity: entity,
          cell: cell,
          password: pass
        });
        sendShopData(data).done(function () {
          if (save) {
            $.cookie("un", cell, { expires: 30, path: '/' });
            $.cookie("pw", pass, { expires: 30, path: '/' });
          }
          shopSuccess();
        }).fail(shopError).always(function () {
          panel.close();
        });
      });
      panelContent.show();
      return;
    }
    var data = JSON.stringify({
      id: id,
      entity: entity,
      cell: un,
      password: pw
    });
    sendShopData(data).done(shopSuccess).fail(shopError);
  });
}

function shopSuccess() {
  var p = $('<p />').html('محصول مورد نظر شما با موفقیت به سبد خرید اضافه شد.');
  var div = $('<div />');
  var commands = $('<div />');
  var a1 = $('<a />').attr({ 'href': shopUrl + '/basket', 'target': '_blank' }).text('مشاهده سبد خرید در فروشگاه آنلاین');
  var a2 = $('<p />').addClass('btn').text('ادامه خرید و مشاهده سایر محصولات');
  commands.append(a1, a2);
  div.append(p, commands);
  var panel = createPanel(div, false, null, null, 320);
  new showPanel(panel, false, true);
}

function shopError(error) {
  var icon = $('<span />').addClass('dkb-triangle-danger');
  var errorPanel = $('<div />').addClass('error-panel-rtl');
  errorPanel.append(icon);
  errorPanel.append($('<h1 />').text('خطا'));
  errorPanel.append($('<p />').html(error));

  var content = createPanel(errorPanel, true, 'خطا هنگام ثبت محصول در سبد خرید', 'dkb-triangle-danger', '80%');
  new showPanel(content, false, true);
}

function sendShopData(data) {
  var deferred = $.Deferred();

  $.ajax({
    type: 'POST',
    url: productApi + 'addToBasket',
    data: data,
    contentType: "application/json; charset=utf-8",
    dataType: "json"
  }).done(function (res) {
    if (res.result)
      deferred.reject(res.result);
    else
      deferred.resolve();
  }).fail(function (request, status, error) {
    var panel = ajaxError(request, status, error);
    showPanel(panel, true, false);
  });

  return deferred.promise();
}

function setupBulkTypes() {
  $('.bulk-type-title').click(function () {
    var panel = $(this);
    var group = panel.parent();
    var isOpen = group.hasClass('open');
    $('.bulk-type-group.open').each(function (index, element) {
      var elem = $(element);
      elem.removeClass('open');
      elem.children().eq(1).slideUp();
    });
    if (!isOpen) {
      group.addClass('open');
      panel.next().slideToggle();
    }
  });
}

function setupMainImage() {
  $('.product-body .image').click(function () {
    showProductImage(this.dataset.href);
  });
}