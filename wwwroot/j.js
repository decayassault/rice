"use strict"

var o = "o";
var T = true;
var H = 300000;
var Z = 0;
var g = "GET";
var P = "POST";
var D = window.document;
var a = undefined;

function w(x, u) {
    x.open(g, u, T);
    x.setRequestHeader("a", a);
    x.timeout = H;
    x.send();
}

function u() {
    m('/l/', 'a');
}

function s() {
    var x = new XMLHttpRequest();
    t(x, function () {
        z();
    });
    w(x, '/y/' + D.getElementsByClassName('s')[Z].innerHTML
        + '?t=' + encodeURIComponent(D.getElementById('x').value), T);
}

function t(x, handler) {
    x.addEventListener("loadend", handler);
}

function m(u, p) {
    var x = new XMLHttpRequest();
    t(x, function () {
        D.getElementById(p).innerHTML = x.responseText;
    });
    w(x, u);
}

function r(c, l, p, e, q) {
    var x = new XMLHttpRequest();
    t(x, function () {
        n('/o/');
    });
    x.open(P, "/g/", T);
    x.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    x.timeout = H;
    x.send("c=" + encodeURIComponent(c) + "&l=" + encodeURIComponent(l)
        + "&p=" + encodeURIComponent(p)
        + "&e=" + encodeURIComponent(e) + "&n=" + encodeURIComponent(q));
}
function c() {
    var e = D.getElementById('u').value;
    var a = D.getElementById('z').value;
    var p = D.getElementById('q').value;
    var b = D.getElementById('b').value;
    if (e === a && p === b)
        r(D.getElementById('y').value,
            D.getElementById('l').value, p, e,
            D.getElementById('e').value);
    else
        alert('Не совпадает повторный ввод пароля или почты.');
}

function n(u) {
    var x = new XMLHttpRequest();
    t(x, function () {
        if (x.status == 200 && x.DONE)
            D.getElementById(o).innerHTML = x.responseText;
    });
    w(x, u);
}

function v(c, l, p) {
    var x = new XMLHttpRequest();
    t(x, function () {
        a = x.responseText;
        z();
        if (a.length === 0)
            alert('Не удалось войти.');
    });
    w(x, "/a/?c=" + encodeURIComponent(c)
        + "&l=" + encodeURIComponent(l) + "&p=" + encodeURIComponent(p));
}

function d() {
    v(D.getElementById('y').value,
        D.getElementById('l').value, D.getElementById('q').value);
}

function q() {
    m('/n/', 't');
}

function l() {
    var x = new XMLHttpRequest();
    t(x, function () {
        z();
    });
    w(x, '/h/' + D.getElementsByClassName('s')[Z].innerHTML
        + '?t=' + encodeURIComponent(D.getElementsByTagName('input')[Z].value)
        + '&m=' + encodeURIComponent(D.getElementById('x').value));
}
function b() {
    n('/d/1');
}
function f() {
    m('/m/', 'a');
}
function e() {
    var x = new XMLHttpRequest();
    t(x, function () {
        z();
    });
    w(x, '/b/' + D.getElementsByClassName('s')[Z].innerHTML
        + '?t=' + encodeURIComponent(D.getElementById('x').value));
}
function k() {
    m('/i/', 'd');
}
function i() {
    var x = new XMLHttpRequest();
    t(x, function () {
        z();
    });
    w(x, '/j/?n=' + encodeURIComponent(D.getElementsByTagName('input')[Z].value)
        + '&m=' + encodeURIComponent(D.getElementById('x').value));
}
function j() {
    n('/o/');
}
function h() {
    n('/r/');
}
function z() {
    n('/c/');
}
