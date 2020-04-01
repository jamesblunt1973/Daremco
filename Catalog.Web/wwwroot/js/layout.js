$(document).ready(function () {
  setupMenu();
  setupMobileMenu();
  setupLinkPanel();
  setupUser();
});

var user;
var productApi = '/api/products/';
var authApi = '/api/auth/';


function setupMenu() {
  var top = $('header');
  var height = top.outerHeight();

  var toTop = $('.scroll-top');
  var body = $(document);
  toTop.click(function () {
    $('html, body').stop().animate({ scrollTop: 0 }, 500, 'swing');
  });

  // body.unbind().scroll(FixMenu);

  var scrollTop = 0, delta = 5, dir = '', prevDir = 'up', toTopVisible = false;
  function FixMenu() {
    var scrollPosition = $(this).scrollTop();

    if (scrollPosition > height) {
      if (!toTopVisible) {
        toTop.addClass('visible');
        toTopVisible = true;
      }
    }
    else if (toTopVisible) {
      toTop.removeClass('visible');
      toTopVisible = false;
    }

    if (scrollPosition > scrollTop + delta)
      dir = 'down';
    else if (scrollPosition < scrollTop - delta)
      dir = 'up';
    else
      return;

    if (scrollPosition >= height) {
      if (dir !== prevDir) {
        if (dir === 'down')
          top.addClass('default').removeClass('fixed');
        else
          top.removeClass('default').addClass('fixed');
        prevDir = dir;
      }
    }
    else {
      top.removeClass('default').removeClass('fixed');
      prevDir = 'up';
    }
    scrollTop = scrollPosition;
  }

}

function setupMobileMenu() {
  var menuBtn = $('.hamburger');
  var body = $('body');
  var menu = $('.menu ul');
  menuBtn.click(function () {
    menuBtn.toggleClass('open');
    menu.toggleClass('open close').fadeToggle();
    if (menuBtn.hasClass('open'))
      body.css('overflow', 'hidden');
    else
      body.css('overflow', '');
  });
}

function showPanel(content, closable, removable, callback) {
  var layer = $('<div />').addClass('layer').hide();

  setContent(content);
  setClosable(closable, removable);

  var body = $('body');
  body.append(layer);
  body.css('overflow', 'hidden');
  var container = body.children('#container').addClass('blur');
  layer.children().hide();
  layer.fadeIn('fast', function () {
    layer.children().slideDown('fast');
  });

  this.close = close;
  function close() {
    if (callback)
      callback();
    layer.children().slideUp('fast', function () {
      layer.fadeOut(function () {
        body.css('overflow', '');
        container.removeClass('blur');
        layer.remove();
      });
    });
  }

  this.setClosable = setClosable;
  function setClosable(closable, removable) {
    var closeBtn = layer.find('.close');
    if (closable)
      closeBtn.click(close);
    else
      closeBtn.unbind('click');
    if (removable)
      layer.click(close);
    else
      layer.unbind('click');
  }

  this.setContent = setContent;
  function setContent(content) {
    layer.html('').append(content);
  }
}

function createPanel(content, closable, title, icon, maxWidth) {
  var panel = $('<div />').addClass('panel');
  if (maxWidth)
    panel.css('max-width', maxWidth);

  var panelHeadr = $('<div />').addClass('panel-header');
  if (icon) {
    panelHeadr.append($('<span />').addClass(icon));
  }
  if (title) {
    panelHeadr.append(title);
  }
  if (closable) {
    var closeBtn = $('<div />').addClass('close');
    closeBtn.attr({ 'title': 'بستن' });
    closeBtn.append($('<span />').addClass('dkb-window-close'));
    panelHeadr.append(closeBtn);
  }
  panel.append(panelHeadr);

  var panelContent = $('<div />').addClass('panel-content');
  panelContent.css({ 'max-height': $(window).outerHeight() - 70, 'overflow': 'auto' });
  panelContent.append(content);
  panel.append(panelContent);

  return panel;
}

function setupLinkPanel() {
  $('a.panel').on('click', function (e) {
    e.preventDefault();
    var a = this;
    if (a.href === '#' || a.href === '' || a.href === window.location.href)
      return;
    var panel = new showPanel(getLoading(), false, false);
    var href = a.href;
    //if (a.hostname !== window.location.hostname)
    //    href = 'https://cors-anywhere.herokuapp.com/' + a.href;
    var jqXhr = $.get(href).done(function (data) {
      var type = jqXhr.getResponseHeader('content-type') || '';
      switch (type) {
        case 'image/png':
        case 'image/jpg':
        case 'image/jpeg':
          var img = $('<img />').attr({ 'src': a.href, 'class': 'center' }).hide();
          img.on('load', function () {
            var width = img.width();
            var height = img.height();
            img.css({ width: 1, height: 1 });
            img.show();
            img.animate({ width: width, height: height }, 300, 'swing');
          });
          panel.setClosable(false, true);
          panel.setContent(img);
          break;
        default:
          var content = createPanel(data, true, a.innerHTML, '', '80%');
          panel.setContent(content);
          panel.setClosable(true, false);
      }
    }).fail(function (request, status, error) {
      panel.setContent(ajaxError(request, status, error));
      panel.setClosable(true, false);
    });
  });
}

function getLoading() {
  return "<div class='lds-ellipsis'><div></div><div></div><div></div><div></div></div>";
}

function ajaxError(request, status, error) {
  var icon = $('<span />').addClass('dkb-triangle-danger');
  var errorPanel = $('<div />').addClass('error-panel');
  errorPanel.append(icon);
  errorPanel.append($('<h1 />').text(error));
  errorPanel.append($('<h2 />').text(status + ' ' + request.status));
  var message = '';
  if (request.responseJSON) {
    if (request.responseJSON.Message)
      message += request.responseJSON.Message + '<br />';
    if (request.responseJSON.ExceptionMessage)
      message += request.responseJSON.ExceptionMessage + '<br />';
    if (request.responseJSON.title)
      message += request.responseJSON.title;
  }
  else
    message = request.responseText;
  errorPanel.append($('<p />').html(message));

  var content = createPanel(errorPanel, true, 'Server Communication Error', 'dkb-triangle-danger', '80%');
  return content;
}

// jquery plugin for modify serialize function behavior regardless of checkboxes
(function ($) {
  $.fn.serialize = function (options) {
    return $.param(this.serializeArray(options));
  };

  $.fn.serializeArray = function (options) {
    var o = $.extend({
      checkboxesAsBools: false
    }, options || {});

    var rselectTextarea = /select|textarea/i;
    var rinput = /text|hidden|password|search|number/i;

    return this.map(function () {
      return this.elements ? $.makeArray(this.elements) : this;
    })
      .filter(function () {
        return this.name && !this.disabled &&
          (this.checked
            || o.checkboxesAsBools && this.type === 'checkbox'
            || rselectTextarea.test(this.nodeName)
            || rinput.test(this.type));
      })
      .map(function (i, elem) {
        var val = $(this).val();
        return val === null ?
          null :
          $.isArray(val) ?
            $.map(val, function (val, i) {
              return { name: elem.name, value: val };
            }) :
            {
              name: elem.name,
              value: o.checkboxesAsBools && this.type === 'checkbox' ?
                this.checked ? true : false :
                val
            };
      }).get();
  };
})(jQuery);

function modifyQueryString(val, key) {
  var qs = window.location.search;
  var qsArray = [];
  if (qs)
    qsArray = qs.substr(1).split('&');
  var index = qsArray.findIndex(function (str) {
    var items = str.split('=');
    return items[0] === key;
  });
  if (val) {
    // اضافه به کوئری استرینگ
    if (index > -1) // key found in the collection
      qsArray[index] = key + '=' + val;
    else // key not found in the collection, so push it
      qsArray.push(key + '=' + val);
  }
  else {
    // حذف از کوئری استرینگ
    if (index > -1)
      qsArray.splice(index);
  }
  return '?' + qsArray.join('&');
}

function formatCurrency(num) {
  var abs = Math.abs(num);
  var sp = $("<span />").text(abs);
  sp.formatCurrency({ roundToDecimalPlace: 0 });
  var str = sp.text();
  if (num < 0)
    str = "-" + str;
  return str;
}

function showProductImage(src, callback) {
  var mainImg = $('<img />').attr('src', src);
  var header = $('<div />').addClass('img-panel-header');
  var content = $('<div />').addClass('img-panel-content');
  content.append(mainImg);
  var imgPanel = $('<div />').addClass('img-panel').append(header, content);
  showPanel(imgPanel, false, true, callback);
}

function setupUser() {
  var userBtn = $('.user');
  var un = $.cookie("un");
  var pw = $.cookie("pw");
  if (un && pw)
    login(un, pw).done(function (res) {
      user = res;
      setupUserIcon();
    });
  userBtn.click(function () {
    if (!user) {
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

        login(cell, pass).done(function (res) {
          if (save) {
            $.cookie("un", cell, { expires: 30, path: '/' });
            $.cookie("pw", pass, { expires: 30, path: '/' });
          }
          user = res;
          setupUserIcon();
        }).fail(shopError).always(function () {
          panel.close();
        });
      });
      panelContent.show();
    }
    else {
      user = undefined;
      $.removeCookie("un", { path: '/' });
      $.removeCookie("pw", { path: '/' });
      setupUserIcon();
    }
  });
}

function login(un, pw) {
  var deferred = $.Deferred();

  var data = {
    userName: un,
    password: pw
  };
  $.ajax({
    type: 'POST',
    url: authApi,
    data: JSON.stringify(data),
    contentType: "application/json; charset=utf-8",
    dataType: "json"
  }).done(function (res) {
    deferred.resolve(res);
  }).fail(function (request, status, error) {
    var panel = ajaxError(request, status, error);
    showPanel(panel, true, false);
  });

  return deferred.promise();

}

function setupUserIcon() {
  var userBtn = $('.user');
  if (user) {
    var imgSrc = usersPath + '_' + user.id + '.jpg';
    userBtn.children('p').text(user.name);
    var img = $('<img />');
    img.on('load', function () {
      userBtn.children('span').hide();
      userBtn.children('img').attr('src', imgSrc).show();
    });
    img.attr('src', imgSrc);
  }
  else {
    userBtn.children('p').text("ورود/ثبت نام");
    userBtn.children('span').show();
    userBtn.children('img').hide();
  }
}