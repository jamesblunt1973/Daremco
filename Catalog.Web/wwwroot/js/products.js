$(document).ready(function () {
  setupFilterPanel();
  setupPictureSize();
  checkPicturesLoaded();
  setupCategories();
  setupSelectControls();
  setupImage();
});

function setupFilterPanel() {
  var filterPanel = $('.filter-panel');
  var body = $('body');

  filterPanel.children('.close').click(function () {
    if (filterPanel.hasClass('show')) {
      filterPanel.toggleClass('show hide');
      window.setTimeout(function () {
        filterPanel.removeClass('hide');
      }, 500);
      body.css('overflow', '');
    }
    else {
      filterPanel.addClass('show');
      body.css('overflow', 'hidden');
    }
  });

  $('#filter-apply').click(function (e) {
    e.preventDefault();
    var form = $(this).parents('form').eq(0);
    var attributeIds = $('input.attribute:checked').map(function () {
      return this.value;
    }).get().join();
    if (attributeIds !== '') {
      var attribute = $('<input />').attr({ type: 'hidden', value: attributeIds, name: 'attributes' });
      form.append(attribute);
    }
    var str = form.find('[name]').filter(function () {
      if (this.type === 'checkbox')
        return this.checked;
      var hasValue = this.value !== undefined && this.value !== null && this.value !== '' && !this.value.startsWith('NaN');
      return hasValue;
    }).serialize({
      checkboxesAsBools: true
    });
    if (str !== '')
      window.location.search = str;
  });

  $('#filter-reset').click(function (e) {
    e.preventDefault();
    if (window.location.search)
      window.location = '/';
  });

  $('.slider-input').jRange({
    format: rangeCallback,
    width: '100%',
    theme: 'theme-blue',
    showLabels: true,
    isRange: true
  });

  var moreBtn = filterPanel.find('.more');
  var plus = moreBtn.children().first();
  var minus = plus.next();
  moreBtn.click(function () {
    moreBtn.next().slideToggle();
    plus.toggle();
    minus.toggle();
  }).click();
}

function rangeCallback(value, pointer) {
  if (!value)
    return '';
  else if (pointer === 'low')
    return 'از: ' + value;
  else
    return 'تا: ' + value;
}

function setupPictureSize() {
  $('.picture-size .picture-size-btn').click(function () {
    var btn = $(this);
    if (btn.hasClass('active'))
      return;
    // get new size
    var newSize = this.dataset.size;
    // change active class
    btn.parent().children().removeClass('active');
    btn.addClass('active');
    // change products picture
    $('.products .product .image').each(function (index, element) {
      var imgPanel = $(element);
      var productPanel = imgPanel.parent();
      var productId = productPanel.attr('data-id');
      imgPanel.html(''); // remove img inside of it
      imgPanel.css('background-image', 'url(\'/img/product-loading.gif\')');

      var src = productsPath + productId + '/' + newSize + '.jpg';
      var img = document.createElement('img');
      img.src = src;
      img.onload = function () {
        imgPanel.append(img);
        imgPanel.css('background-image', '');
      };
      img.onerror = function () {
        imgPanel.css('background-image', 'url(\'/img/product.png\')');
      };
      productPanel.removeClass('p160 p200 p240 p300').addClass('p' + newSize);
    });
    // set cookie
    var expiryDate = new Date();
    expiryDate.setMonth(expiryDate.getMonth() + 1);
    document.cookie = 'picture-size=' + newSize + '; expires=' + expiryDate.toUTCString();
    $('html, body').stop().animate({ scrollTop: 0 }, 500, 'swing');
  });
}

function checkPicturesLoaded() {
  window.setTimeout(function () {
    $('.product .image img').each(function (index, img) {
      if (img.naturalWidth === 0) // اگر تصویر محصول لود نشد، عکس پیش فرض به جای آن نمایش داده می‌شود
      {
        var imgObj = $(img);
        var a = imgObj.parent();
        a.addClass('no-pic').attr('href', '');
        imgObj.hide();
      }
    });
  }, 2000);
}

function setupCategories() {
  $.getJSON(productApi + 'categories').done(function (categories) {
    var categoriesSelect = $('#categoryId');
    var selectedCategoryValue = categoriesSelect.attr('data-value');
    var galleriesSelect = $('#galleryId');
    var selectedGalleryValue = galleriesSelect.attr('data-value');
    for (var i in categories) {
      var category = categories[i];
      var o = $('<option />').val(category.Id).text(category.Name).data('category', category);
      categoriesSelect.append(o);
    }
    categoriesSelect.change(function () {
      galleriesSelect.html('');
      galleriesSelect.append($('<option />').val('').text('انتخاب گالری'));
      var selectedCategory = categoriesSelect.children(':selected');
      if (!selectedCategory.val())
        return;
      galleries = selectedCategory.data('category').Galleries;
      for (var i in galleries) {
        var gallery = galleries[i];
        var o = $('<option />').val(gallery.Id).text(gallery.Name);
        galleriesSelect.append(o);
      }
    });
    if (selectedCategoryValue) {
      categoriesSelect.val(selectedCategoryValue);
      categoriesSelect.change();
    }
    if (selectedGalleryValue) {
      galleriesSelect.val(selectedGalleryValue);
    }
  });
}

function setupSelectControls() {
  var controls = $('.select-control');
  var select1 = controls.eq(0);
  var key1 = select1.attr('name');
  var select2 = controls.eq(1);
  var key2 = select2.attr('name');
  controls.change(function () {
    var qs = '';
    var id1 = select1.val();
    var id2 = select2.val();

    if (id1) {
      qs = '?' + key1 + '=' + id1;
      if (id2 && this.name !== key1)
        qs += '&' + key2 + '=' + id2;
    }
    window.location.search = qs;
  });
}

function setupImage() {
  var filterPanel = $('.filter-panel');
  $('.product-list-item .image').click(function () {
    filterPanel.hide();
    showProductImage(this.dataset.href, function () {
      filterPanel.show();
    });
  });
}