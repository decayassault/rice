var o = "o",
    T = true,
    H = 300000,
    K = 15000,
    I = "Форум любви",
    Z = 0,
    g = "GET",
    P = "POST",
    D = window.document,
    a = undefined,
    N = 0,
    S = [],
    M = [0],
    J = [[Z, '', Y()]],
    U = '/';

A('/c/');
J[Z][1] = D.getElementById(o).innerHTML;

function V() {
    var s = 's';
    var r = D.getElementById('fo').classList;
    var q = D.getElementById('ba').classList;
    var l = M.length;

    if (N === 0) {
        if (l > 1) {
            if (r.contains(s))
                r.remove(s);
        }
        else
            if (!r.contains(s))
                r.add(s);

        if (!q.contains(s))
            q.add(s);
    }
    else if (N === l - 1) {
        if (q.contains(s))
            q.remove(s);

        if (!r.contains(s))
            r.add(s);
    }
    else {
        if (q.contains(s))
            q.remove(s);
        if (r.contains(s))
            r.remove(s);
    }
}

function R(x) {
    t(x, function () {
        z();
    });
}

function A(e) {
    var l = M.length;
    var p = S.indexOf(e);

    if (p === -1) {
        p = S.length;
        S.push(e);
    }

    if (M[l - 1] != p) {
        M.push(p);
        N = l;
        V();
    }
}

function w(x, u, e) {
    if (e === undefined) {
        x.open(g, u, T);
        x.setRequestHeader("a", a);
        x.timeout = H;
        x.send();
    }
    else {
        x.open(P, u, T);
        x.setRequestHeader("a", a);
        x.timeout = H;
        var f = new FormData();
        f.append("f", e);
        x.send(f);
    }
}

function u() {
    m('/l/', 'a');
}

function L() {
    var a = M[N];
    n(S[a], a, 1);
}

function s() {
    var x = X();
    R(x);
    w(x, '/y/' + D.getElementById('o').firstChild.innerHTML
        + '?t=' + encodeURIComponent(D.getElementById('x').value));
}

function y() {
    var i = D.querySelectorAll('link[rel=stylesheet]');
    i[1].disabled = !i[1].disabled;
    i[2].disabled = !i[2].disabled;
}

function t(x, handler) {
    x.addEventListener("loadend", handler);
}

function m(u, p) {
    var x = X();
    t(x, function () {
        D.getElementById(p).innerHTML = x.responseText;
    });
    w(x, u);
}

function B() {
    if (N > 0) {
        N = N - 1;
        L();
        V();
    }
}

function F() {
    if (N < M.length - 1) {
        N = N + 1;
        L();
        V();
    }
}

function r(c, l, p, e, q) {
    var x = X();
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
        alert('Не совпадает повторный ввод пароля или секретного слова.');
}

function n(u, e, s) {
    var d = undefined;
    var l = J.length;

    if (e != undefined) {
        var p = Y(u);

        for (var i = Z; i < l; i++) {
            var b = i;
            var c = J[b];

            if (c[Z] === e) {
                if (Y() - c[2] >= K) {
                    Q(u, b, 1, undefined, s);
                    J[b][2] = p;
                }
                else {
                    D.getElementById(o).innerHTML = c[1];
                }

                d = 1;

                break;
            }
        }

        if (!d) {
            Q(u, l, undefined, undefined, s);
        }
    }
    else {
        var b = S.length;

        for (var i = Z; i < b; i++)
            if (S[i] === u) {
                var j = i;
                n(u, j, s);
                d = 1;

                break;
            }

        if (!d)
            Q(u, l, undefined, b, s);
    }
}

function X() {
    return new XMLHttpRequest();
}

function Q(u, i, a, s, l) {
    var x = X();
    t(x, function () {
        if (x.status == 200 && x.DONE) {
            var f = x.responseText;

            if (l != 1)
                A(u);

            if (a === 1) {
                J[i][1] = f;
            }
            else {
                J.push([s, f, Y(u)]);
            }
            D.getElementById(o).innerHTML = f;
        }
    });
    w(x, u);
}

function Y(u) {
    return (['/o/', '/r/'].indexOf(u) === -1) ? new Date().getTime() : 0;
}

function v(c, l, p) {
    var x = X();
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
    var x = X();
    R(x);
    w(x, '/h/' + D.getElementById('o').lastChild.innerHTML
        + '?t=' + encodeURIComponent(D.getElementsByTagName('input')[Z].value)
        + '&m=' + encodeURIComponent(D.getElementById('x').value));
}
function b() {
    n('/d/1');
}
function f() {
    m('/m/', 'a');
}
function p(e) {
    if (e === undefined)
        n('/q/');
    else {
        var x = X();
        R(x);
        var i = D.getElementsByTagName('input');
        var d = "";
        var l = i.length - 2;
        var s = '&b=';

        for (var k = 1; k <= l; k += 2)
            if (i[k].checked ^ i[k + 1].checked)
                d = d + s + i[k].checked;

        w(x, '/k/' + e
            + '?t=' + encodeURIComponent(D.getElementById('x').value)
            + d,
            D.getElementById('f').files[Z]);
    }
}
function e() {
    var x = new X();
    R(x);
    w(x, '/b/' + D.getElementById('o').firstChild.innerHTML
        + '?t=' + encodeURIComponent(D.getElementById('x').value));
}
function k() {
    m('/i/', 'd');
}
function i() {
    var x = X();
    R(x);
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
