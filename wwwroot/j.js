"use strict"

var o = "o";
var t = true;
var H = 300000;//production 30000
var z = 0;
var g = "GET";
var P = "POST";
var D = window.document;
var a = undefined;

function u() {
    m('/l/', 'a');
}

function s() {
    var x = new XMLHttpRequest();
    x.open(g, '/y/' + D.getElementsByClassName('s')[z].innerHTML + '?t=' + encodeURIComponent(D.getElementById('x').value), t);
    x.setRequestHeader("a", a);
    x.timeout = H;
    x.send();
}

function shake(x, handler) {
    x.addEventListener("loadend", handler);//does not work in IE
}

function m(url, target) {
    var x = new XMLHttpRequest();
    shake(x, function () {
        D.getElementById(target).innerHTML = x.responseText;
    });
    x.open(g, url, t);
    x.setRequestHeader("a", a);
    x.timeout = H;
    x.send();
}

function register(captcha, login, password, email, nick) {
    var url = "/g/";
    var x = new XMLHttpRequest();
    shake(x, function () {
        n('/o/');
    });
    x.open(P, url, t);
    x.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    x.timeout = H;
    x.send("c=" + encodeURIComponent(captcha) + "&l=" + encodeURIComponent(login) + "&p=" + encodeURIComponent(password)
        + "&e=" + encodeURIComponent(email) + "&n=" + encodeURIComponent(nick));
}

function p() {
    n('/c/');
}

function c() {
    var captcha = D.getElementById('captcha').value;
    var nick = D.getElementById('nick').value;
    var email = D.getElementById('email').value;
    var emailconfirm = D.getElementById('emailconfirm').value;
    var login = D.getElementById('l').value;
    var pwd = D.getElementById('password').value;
    var pwdconfirm = D.getElementById('pwdconfirm').value;
    register(captcha, login, pwd, email, nick);
}

function n(url) {
    var x = new XMLHttpRequest();
    shake(x, function () {
        if (x.status == 200 && x.DONE)
            D.getElementById(o).innerHTML = x.responseText;
    });
    x.open(g, url, t);
    x.setRequestHeader("a", a);
    x.timeout = H;
    x.send();
}

function auth(captcha, login, password) {
    var url = "/a/?c=" + encodeURIComponent(captcha)
        + "&l=" + encodeURIComponent(login) + "&p=" + encodeURIComponent(password);
    var x = new XMLHttpRequest();
    shake(x, function () {
        a = x.responseText;
        n('/c/');
        if (a.length === 0)
            alert('Не удалось войти.');
    });
    x.open(g, url, t);
    x.setRequestHeader("a", a);
    x.timeout = H;
    x.send();
}

function d() {
    var captcha = D.getElementById('captcha').value;
    var login = D.getElementById('l').value;
    var password = D.getElementById('password').value;
    auth(captcha, login, password);
}

function newTopic() {
    m('/n/', 't');
}

function startTopic() {
    var x = new XMLHttpRequest();
    shake(x, function () {
        n('/c/');
    });
    x.open(g, '/h/' + D.getElementsByClassName('s')[z].innerHTML + '?t=' + encodeURIComponent(D.getElementsByTagName('input')[z].value) + '&m=' + encodeURIComponent(D.getElementById('x').value), t);
    x.setRequestHeader("a", a);
    x.timeout = H;
    x.send();

    //TODO
}
function openDialogsList() {
    n('/d/1');
}
function replyPM() {
    m('/m/', 'a');
}
function sendPrivateReply() {
    var x = new XMLHttpRequest();
    shake(x, function () {
        n('/c/');
    });
    x.open(g, '/b/' + D.getElementsByClassName('s')[z].innerHTML + '?t=' + encodeURIComponent(D.getElementById('x').value), t);
    x.setRequestHeader("a", a);
    x.timeout = H;
    x.send();
}
function newDialog() {
    m('/i/', 'd');
}
function startDialog() {
    var x = new XMLHttpRequest();
    shake(x, function () {
        n('/c/');
    });
    x.open(g, '/j/?n=' + encodeURIComponent(D.getElementsByTagName('input')[z].value) + '&m='
        + encodeURIComponent(D.getElementById('x').value), t);
    x.setRequestHeader("a", a);
    x.timeout = H;
    x.send();
    //TODO
}
function j() {
    n('/o/');
}
function h() {
    n('/r/');
}
