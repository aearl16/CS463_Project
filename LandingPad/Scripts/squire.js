﻿!function (e, t) {
    "use strict";
    function n(e, t, n) {
        this.root = this.currentNode = e,
            this.nodeType = t,
            this.filter = n;
    }

    function o(e, t) {
        for (var n = e.length; n--;) {
            if (!t(e[n]))
                return !1;

            return !0;
        }
    }

    function i(e) {
        return e.nodeType === w && !!ge[e.nodeName];
    }

    function r(e) {
        switch (e.nodeType) {
            case F: return ve;
            case w: case H: if (ce && Ce.has(e)) return Ce.get(e);
                break;
            default: return me;
        }

        var t;

        return t = o(e.childNodes, a) ? pe.test(e.nodeName) ? ve : _e : Ne, ce && Ce.set(e, t), t;
    }

    function a(e) {
        return r(e) === ve;
    }

    function s(e) {
        return r(e) === _e;
    }

    function d(e) {
        return r(e) === Ne;
    }

    function l(e, t) {
        var o = new n(t, W, s);

        return o.currentNode = e, o;
    }

    function c(e, t) {
        return e = l(e, t).previousNode(), e !== t ? e : null;
    }

    function h(e, t) {
        return e = l(e, t).nextNode(), e !== t ? e : null;
    }

    function u(e) {
        return !e.textContent && !e.querySelector("IMG");
    }

    function f(e, t) {
        return !i(e) && e.nodeType === t.nodeType && e.nodeName === t.nodeName && "A" !== e.nodeName && e.className === t.className && (!e.style && !t.style || e.style.cssText === t.style.cssText);
    }

    function p(e, t, n) {
        if (e.nodeName !== t)
            return !1;

        for (var o in n) {
            if (e.getAttribute(o) !== n[o])
                return !1;
        }
        return !0;
    }

    function g(e, t, n, o) {
        for (; e && e !== t;) {
            if (p(e, n, o))
                return e;

            e = e.parentNode;
        }

        return null;
    }

    function m(e, t) {
        for (; t;) {
            if (t === e)
                return !0;

            t = t.parentNode;
        }

        return !1;
    }

    function v(e, t) {
        var n, o, i, r, a = "";

        return e && e !== t && (a = v(e.parentNode, t),
            e.nodeType === w && (a += (a ? ">" : "") + e.nodeName,
                (n = e.id) && (a += "#" + n),
                (o = e.className.trim()) && (i = o.split(/\s\s*/), i.sort(), a += ".", a += i.join(".")),
                (r = e.dir) && (a += "[dir=" + r + "]"),
                i && (ue.call(i, z) > -1 && (a += "[backgroundColor=" + e.style.backgroundColor.replace(/ /g, "") + "]"),
                    ue.call(i, q) > -1 && (a += "[color=" + e.style.color.replace(/ /g, "") + "]"),
                    ue.call(i, K) > -1 && (a += "[fontFamily=" + e.style.fontFamily.replace(/ /g, "") + "]"),
                    ue.call(i, G) > -1 && (a += "[fontSize=" + e.style.fontSize + "]")))),
            a;
    }

    function _(e) {
        var t = e.nodeType;

        return t === w || t === H ? e.childNodes.length : e.length || 0;
    }

    function N(e) {
        var t = e.parentNode;

        return t && t.removeChild(e), e;
    }

    function C(e, t) {
        var n = e.parentNode;

        n && n.replaceChild(t, e);
    }

    function S(e) {
        for (var t = e.ownerDocument.createDocumentFragment(), n = e.childNodes, o = n ? n.length : 0; o--;)
            t.appendChild(e.firstChild);

        return t;
    }

    function T(e, n, o, i) {
        var r, a, s, d = e.createElement(n);

        if (o instanceof Array && (i = o, o = null), o) {
            for (r in o)
                o[r] !== t && d.setAttribute(r, o[r]);
        }

        if (i) {
            for (a = 0, s = i.length; a < s; a += 1)
                d.appendChild(i[a]);
        }

        return d;
    }

    function y(e, t) {
        var n, o, r = t.__squire__, s = e.ownerDocument, d = e;

        if (e === t && ((o = e.firstChild) && "BR" !== o.nodeName || (n = r.createDefaultBlock(),
            o ? e.replaceChild(n, o) : e.appendChild(n),
            e = n,
            n = null)),
            e.nodeType === F)
            return d;

        if (a(e)) {
            for (o = e.firstChild; se && o && o.nodeType === F && !o.data;)
                e.removeChild(o), o = e.firstChild; o || (se ? (n = s.createTextNode(Z), r._didAddZWS()) : n = s.createTextNode(""));
        }
        else if (ae) {
            for (; e.nodeType !== F && !i(e);) {
                if (!(o = e.firstChild)) {
                    n = s.createTextNode("");
                    break;
                }

                e = o;
            }

            e.nodeType === F ? /^ +$/.test(e.data) && (e.data = "") : i(e) && e.parentNode.insertBefore(s.createTextNode(""), e);
        }
        else if (!e.querySelector("BR")) {
            for (n = T(s, "BR"); (o = e.lastElementChild) && !a(o);)
                e = o;
        }

        if (n) {
            try {
                e.appendChild(n);
            }
            catch (t) {
                r.didError({
<<<<<<< HEAD
                    name: "Squire: fixCursor – " + t,
=======
                    name: "Squire: fixCursor – " + t,
>>>>>>> d0a117bf3f2087f7d07377a396e42ad05258e8b6
                    message: "Parent: " + e.nodeName + "/" + e.innerHTML + " appendChild: " + n.nodeName
                });
            }

            return d;
        }
    }

    function E(e, t) {
        var n,
            o,
            i,
            r,
            s = e.childNodes,
            l = e.ownerDocument,
            c = null,
            h = t.__squire__._config;

        for (n = 0, o = s.length; n < o; n += 1)
            i = s[n],
                r = "BR" === i.nodeName,
                !r && a(i) ? (c || (c = T(l, h.blockTag, h.blockAttributes)),
                    c.appendChild(i), n -= 1, o -= 1) : (r || c) && (c || (c = T(l, h.blockTag, h.blockAttributes)),
                        y(c, t), r ? e.replaceChild(c, i) : (e.insertBefore(c, i), n += 1, o += 1),
                        c = null),
                d(i) && E(i, t);

        return c && e.appendChild(y(c, t)), e;
    }

    function b(e, t, n, o) {
        var i, r, a, s = e.nodeType;

        if (s === F && e !== n)
            return b(e.parentNode, e.splitText(t), n, o);

        if (s === w) {
            if ("number" === typeof t && (t = t < e.childNodes.length ? e.childNodes[t] : null), e === n)
                return t;

            for (i = e.parentNode, r = e.cloneNode(!1); t;)
                a = t.nextSibling, r.appendChild(t), t = a;

            return "OL" === e.nodeName && g(e, o, "BLOCKQUOTE") && (r.start = (+e.start || 1) + e.childNodes.length - 1),
                y(e, o),
                y(r, o),
                (a = e.nextSibling) ? i.insertBefore(r, a) : i.appendChild(r),
                b(i, r, n, o);
        }

        return t;
    }

    function k(e, t) {
        for (var n, o, i, r = e.childNodes, s = r.length, d = []; s--;) {
            if (n = r[s], o = s && r[s - 1], s && a(n) && f(n, o) && !ge[n.nodeName]) {
                t.startContainer === n && (t.startContainer = o, t.startOffset += _(o)),
                    t.endContainer === n && (t.endContainer = o,
                        t.endOffset += _(o)),
                    t.startContainer === e && (t.startOffset > s ? t.startOffset -= 1 : t.startOffset === s && (t.startContainer = o,
                        t.startOffset = _(o))),
                    t.endContainer === e && (t.endOffset > s ? t.endOffset -= 1 : t.endOffset === s && (t.endContainer = o,
                        t.endOffset = _(o))),
                    N(n),
                    n.nodeType === F ? o.appendData(n.data) : d.push(S(n));
            }
            else if (n.nodeType === w) {
                for (i = d.length; i--;)
                    n.appendChild(d.pop());

                k(n, t);
            }
        }

        function L(e, t) {
            if (e.nodeType === F && (e = e.parentNode), e.nodeType === w) {
                var n = { startContainer: t.startContainer, startOffset: t.startOffset, endContainer: t.endContainer, endOffset: t.endOffset };

                k(e, n), t.setStart(n.startContainer, n.startOffset), t.setEnd(n.endContainer, n.endOffset);
            }
        }

        function x(e, t, n, o) {
            for (var i, r, a, s = t; (i = s.parentNode) && i !== o && i.nodeType === w && 1 === i.childNodes.length;)
                s = i;

            N(s), a = e.childNodes.length,
                r = e.lastChild, r && "BR" === r.nodeName && (e.removeChild(r), a -= 1),
                e.appendChild(S(t)),
                n.setStart(e, a),
                n.collapse(!0),
                L(e, n),
                te && (r = e.lastChild) && "BR" === r.nodeName && e.removeChild(r);
        }

        function O(e, t) {
            var n,
                o,
                i = e.previousSibling,
                r = e.firstChild,
                a = e.ownerDocument,
                s = "LI" === e.nodeName;

            if (!s || r && /^[OU]L$/.test(r.nodeName)) {
                if (i && f(i, e)) {
                    if (!d(i)) {
                        if (!s)
                            return;

                        o = T(a, "DIV"), o.appendChild(S(i)), i.appendChild(o);
                    }

                    N(e), n = !d(e), i.appendChild(S(e)), n && E(i, t), r && O(r, t);
                }
                else
                    s && (i = T(a, "DIV"), e.insertBefore(i, r), y(i, t));
            }
        }

        function A(e) {
            this.isShiftDown = e.shiftKey;
        }

        function B(e, t, n) {
            var o, i;

            if (e || (e = {}), t)
                for (o in t)
                    !n && o in e || (i = t[o], e[o] = i && i.constructor === Object ? B(e[o], i, n) : i);

            return e;
        }

        function R(e, t) {
            e.nodeType === M && (e = e.body);
            var n, o = e.ownerDocument, i = o.defaultView;

            this._win = i,
                this._doc = o,
                this._root = e,
                this._events = {},
                this._isFocused = !1,
                this._lastSelection = null,
                de && this.addEventListener("beforedeactivate", this.getSelection),
                this._hasZWS = !1,
                this._lastAnchorNode = null,
                this._lastFocusNode = null,
                this._path = "",
                this._willUpdatePath = !1,
                "onselectionchange" in o ? this.addEventListener("selectionchange",
                    this._updatePathOnEvent) : (this.addEventListener("keyup",
                        this._updatePathOnEvent),
                        this.addEventListener("mouseup",
                            this._updatePathOnEvent)),
                this._undoIndex = -1,
                this._undoStack = [],
                this._undoStackLength = 0,
                this._isInUndoState = !1,
                this._ignoreChange = !1,
                this._ignoreAllChanges = !1,
                le ? (n = new MutationObserver(this._docWasChanged.bind(this)),
                    n.observe(e,
                        {
                            childList: !0,
                            attributes: !0,
                            characterData: !0,
                            subtree: !0
                        }),
                    this._mutation = n) : this.addEventListener("keyup",
                        this._keyUpDetectChange),
                this._restoreSelection = !1,
                this.addEventListener("blur", D),
                this.addEventListener("mousedown", U),
                this.addEventListener("touchstart", U),
                this.addEventListener("focus", P),
                this._awaitingPaste = !1,
                this.addEventListener(ee ? "beforecut" : "cut", nt),
                this.addEventListener("copy", ot),
                this.addEventListener("keydown", A),
                this.addEventListener("keyup", A),
                this.addEventListener(ee ? "beforepaste" : "paste", it),
                this.addEventListener("drop", rt),
                this.addEventListener(te ? "keypress" : "keydown", we),
                this._keyHandlers = Object.create(We),
                this.setConfig(t),
                ee && (i.Text.prototype.splitText = function (e) {
                    var t = this.ownerDocument.createTextNode(this.data.slice(e)),
                        n = this.nextSibling, o = this.parentNode,
                        i = this.length - e;

                    return n ? o.insertBefore(t, n) : o.appendChild(t),
                        i && this.deleteData(e, i),
                        t;
                }),
                e.setAttribute("contenteditable", "true");

            try {
                o.execCommand("enableObjectResizing", !1, "false"),
                    o.execCommand("enableInlineTableEditing", !1, "false");
            }
            catch (e) {
                return e;
            }

            e.__squire__ = this, this.setHTML("");
        }

        function D() {
            this._restoreSelection = !0;
        }

        function U() {
            this._restoreSelection = !1;
        }

        function P() {
            this._restoreSelection && this.setSelection(this._lastSelection);
        }

        function I(e, t, n) {
            var o, i;
            for (o = t.firstChild; o; o = i) {
                if (i === o.nextSibling, a(o)) {
                    if (o.nodeType === F || "BR" === o.nodeName || "IMG" === o.nodeName) {
                        n.appendChild(o);
                        continue;
                    }
                }
                else if (s(o)) {
                    n.appendChild(e.createDefaultBlock([I(e, o, e._doc.createDocumentFragment())]));
                    continue;
                }

                I(e, o, n);
            }

            return n;
        }

        var w = 1,
            F = 3,
            M = 9,
            H = 11,
            W = 1,
            z = "highlight",
            q = "colour",
            K = "font",
            G = "size",
            Z = "​",
            j = e.defaultView,
            $ = navigator.userAgent,
            Q = /Android/.test($),
            V = /iP(?:ad|hone|od)/.test($),
            Y = /Mac OS X/.test($),
            X = /Windows NT/.test($),
            J = /Gecko\//.test($),
            ee = /Trident\/[456]\./.test($),
            te = !!j.opera, ne = /Edge\//.test($),
            oe = !ne && /WebKit\//.test($),
            ie = /Trident\/[4567]\./.test($),
            re = Y ? "meta-" : "ctrl-",
            ae = ee || te, se = ee || oe,
            de = ee,
            le = "undefined" !== typeof MutationObserver,
            ce = "undefined" !== typeof WeakMap,
            he = /[^ \t\r\n]/,
            ue = Array.prototype.indexOf;

        Object.create || (Object.create = function (e) {
            var t = function () { };
            return t.prototype = e, new t;
        });

        var fe = { 1: 1, 2: 2, 3: 4, 8: 128, 9: 256, 11: 1024 };

        n.prototype.nextNode = function () {
            for (var e, t = this.currentNode, n = this.root, o = this.nodeType, i = this.filter; ;) {
                for (e = t.firstChild; !e && t && t !== n;) {
                    (e = t.nextSibling) || (t = t.parentNode);
                    if (!e)
                        return null;

                    if (fe[e.nodeType] & o && i(e))
                        return this.currentNode = e, e;

                    t = e;
                }
            }
        },
            n.prototype.previousNode = function () {
                for (var e, t = this.currentNode, n = this.root, o = this.nodeType, i = this.filter; ;) {
                    if (t === n) return null;

                    if (e === t.previousSibling)
                        for (; t === e.lastChild;)e = t;
                    else
                        e = t.parentNode;

                    if (!e)
                        return null;

                    if (fe[e.nodeType] & o && i(e))
                        return this.currentNode = e, e;

                    t = e;
                }
            },
            n.prototype.previousPONode = function () {
                for (var e, t = this.currentNode, n = this.root, o = this.nodeType, i = this.filter; ;) {
                    for (e = t.lastChild; !e && t && t !== n;) {
                        (e = t.previousSibling) || (t = t.parentNode);

                        if (!e)
                            return null;

                        if (fe[e.nodeType] & o && i(e))
                            return this.currentNode = e, e;

                        t = e;
                    }
                }
            };

        var pe = /^(?:#text|A(?:BBR|CRONYM)?|B(?:R|D[IO])?|C(?:ITE|ODE)|D(?:ATA|EL|FN)|EM|FONT|HR|I(?:FRAME|MG|NPUT|NS)?|KBD|Q|R(?:P|T|UBY)|S(?:AMP|MALL|PAN|TR(?:IKE|ONG)|U[BP])?|TIME|U|VAR|WBR)$/,
            ge = { BR: 1, HR: 1, IFRAME: 1, IMG: 1, INPUT: 1 },
            me = 0,
            ve = 1,
            _e = 2,
            Ne = 3,
            Ce = ce ? new WeakMap : null,
            Se = function (e, t) {
                for (var n = e.childNodes; t && e.nodeType === w;)
                    e = n[t - 1], n = e.childNodes, t = n.length;

                return e;
            },
            Te = function (e, t) {
                if (e.nodeType === w) {
                    var n = e.childNodes;

                    if (t < n.length)
                        e = n[t];
                    else {
                        for (; e && !e.nextSibling;)
                            e = e.parentNode;

                        e && (e = e.nextSibling);
                    }
                }

                return e;
            },
            ye = function (e, t) {
                var n,
                    o,
                    i,
                    r,
                    a = e.startContainer,
                    s = e.startOffset,
                    d = e.endContainer,
                    l = e.endOffset;

                a.nodeType === F ? (n = a.parentNode,
                    o = n.childNodes,
                    s === a.length ? (s = ue.call(o, a) + 1,
                        e.collapsed && (d = n, l = s)) : (s && (r = a.splitText(s), d === a ? (l -= s, d = r) : d === n && (l += 1), a = r),
                            s = ue.call(o, a)), a = n) : o = a.childNodes,
                    i = o.length, s === i ? a.appendChild(t) : a.insertBefore(t, o[s]),
                    a === d && (l += o.length - i),
                    e.setStart(a, s),
                    e.setEnd(d, l);
            },
            Ee = function (e, t, n) {
                var o = e.startContainer,
                    i = e.startOffset,
                    r = e.endContainer,
                    a = e.endOffset;

                t || (t = e.commonAncestorContainer),
                    t.nodeType === F && (t = t.parentNode);

                for (var s, d, l, c = b(r, a, t, n), h = b(o, i, t, n), u = t.ownerDocument.createDocumentFragment(); h !== c;) {
                    s = h.nextSibling,
                        u.appendChild(h),
                        h = s;
                }

                return o = t, i = c ? ue.call(t.childNodes, c) : t.childNodes.length,
                    l = t.childNodes[i], d = l && l.previousSibling,
                    d && d.nodeType === F && l.nodeType === F && (o = d, i = d.length, d.appendData(l.data), N(l)),
                    e.setStart(o, i),
                    e.collapse(!0),
                    y(t, n),
                    u;

            },
            be = function (e, t) {
                var n,
                    o,
                    i = Ae(e, t),
                    r = Be(e, t),
                    a = i !== r;

                return xe(e),
                    Oe(e, i, r, t),
                    n = Ee(e, null, t),
                    xe(e),
                    a && (r = Be(e, t), i && r && i !== r && x(i, r, e, t)),
                    i && y(i, t), o = t.firstChild,
                    o && "BR" !== o.nodeName ? e.collapse(!0) : (y(t, t), e.selectNodeContents(t.firstChild)),
                    n;
            },
            ke = function (e, t, n) {
                var o,
                    i,
                    r,
                    s,
                    l,
                    f,
                    p,
                    m,
                    v,
                    C,
                    S;

                for (E(t, n), o === t; o === h(o, n);)
                    y(o, n);

                if (e.collapsed || be(e, n), xe(e), e.collapse(!1),
                    s = g(e.endContainer, n, "BLOCKQUOTE") || n, i = Ae(e, n),
                    m = h(t, t),
                    p = !!i && u(i),
                    i && m && !p && !g(m, t, "PRE") && !g(m, t, "TABLE")) {

                    if (Oe(e, i, i, n),
                        e.collapse(!0),
                        l = e.endContainer,
                        f = e.endOffset,
                        et(i, n, !1),
                        a(l) && (v = b(l, f, c(l, n), n),
                            l = v.parentNode,
                            f = ue.call(l.childNodes, v)),
                        f !== _(l)) {
                        for (r = n.ownerDocument.createDocumentFragment(); o === l.childNodes[f];)
                            r.appendChild(o);

                        x(l, m, e, n), f = ue.call(l.parentNode.childNodes, l) + 1,
                            l = l.parentNode, e.setEnd(l, f);
                    }
                }

                _(t) && (p && (e.setEndBefore(i), e.collapse(!1),
                    N(i)),
                    Oe(e, s, s, n),
                    v = b(e.endContainer, e.endOffset, s, n),
                    C = v ? v.previousSibling : s.lastChild, s.insertBefore(t, v),
                    v ? e.setEndBefore(v) : e.setEnd(s, _(s)),
                    i = Be(e, n), xe(e),
                    l = e.endContainer,
                    f = e.endOffset,
                    v && d(v) && O(v, n),
                    v = C && C.nextSibling,
                    v && d(v) && O(v, n),
                    e.setEnd(l, f)),
                    r && (S = e.cloneRange(),
                        x(i, r, S, n),
                        e.setEnd(S.endContainer, S.endOffset)),
                    xe(e);
            },
            Le = function (e, t, n) {
                var o = t.ownerDocument.createRange();

                if (o.selectNode(t), n) {
                    var i = e.compareBoundaryPoints(3, o) > -1,
                        r = e.compareBoundaryPoints(1, o) < 1;

                    return !i && !r;
                }

                var a = e.compareBoundaryPoints(0, o) < 1,
                    s = e.compareBoundaryPoints(2, o) > -1;

                return a && s;
            },
            xe = function (e) {
                for (var t,
                    n = e.startContainer,
                    o = e.startOffset,
                    r = e.endContainer,
                    a = e.endOffset,
                    s = !0;
                    n.nodeType !== F && (t = n.childNodes[o]) && !i(t);)
                    n = t, o = 0;

                if (a)
                    for (; r.nodeType !== F;) {
                        if (!(t = r.childNodes[a - 1]) || i(t)) {
                            if (s && t && "BR" === t.nodeName) {
                                a -= 1, s = !1;
                                continue;
                            }

                            break;
                        }

                        r = t, a = _(r);
                    }
                else
                    for (; r.nodeType !== F && (t = r.firstChild) && !i(t);)
                        r = t; e.collapsed ? (e.setStart(r, a), e.setEnd(n, o)) : (e.setStart(n, o), e.setEnd(r, a));
            },
            Oe = function (e, t, n, o) {
                var i,
                    r = e.startContainer,
                    a = e.startOffset,
                    s = e.endContainer,
                    d = e.endOffset,
                    l = !0;

                for (t || (t = e.commonAncestorContainer), n || (n = t); !a && r !== t && r !== o;)
                    i = r.parentNode,
                        a = ue.call(i.childNodes, r),
                        r = i;

                for (; ;) {
                    if (l && s.nodeType !== F && s.childNodes[d] && "BR" === s.childNodes[d].nodeName && (d += 1, l = !1),
                        s === n || s === o || d !== _(s))
                        break;

                    i = s.parentNode,
                        d = ue.call(i.childNodes, s) + 1, s = i;
                }

                e.setStart(r, a),
                    e.setEnd(s, d);
            },
            Ae = function (e, t) {
                var n, o = e.startContainer;
                return a(o) ? n = c(o, t) : o !== t && s(o) ? n = o : (n = Se(o, e.startOffset), n = h(n, t)), n && Le(e, n, !0) ? n : null;
            },
            Be = function (e, t) {
                var n, o, i = e.endContainer;

                if (a(i))
                    n = c(i, t);
                else if (i !== t && s(i))
                    n = i;
                else {
                    if (!(n = Te(i, e.endOffset)) || !m(t, n)) {
                        for (n = t; o === n.lastChild;)
                            n = o;
                    }

                    n = c(n, t);
                }

                return n && Le(e, n, !0) ? n : null;
            },
            Re = new n(null, 4 | W, function (e) {
                return e.nodeType === F ? he.test(e.data) : "IMG" === e.nodeName;
            }),
            De = function (e, t) {
                var n, o = e.startContainer, i = e.startOffset;

                if (Re.root = null, o.nodeType === F) {
                    if (i)
                        return !1;

                    n = o;
                }
                else if (n = Te(o, i), n && !m(t, n) && (n = null), !n && (n = Se(o, i), n.nodeType === F && n.length))
                    return !1;

                return Re.currentNode = n, Re.root = Ae(e, t), !Re.previousNode();
            },
            Ue = function (e, t) {
                var n, o = e.endContainer, i = e.endOffset;

                if (Re.root = null, o.nodeType === F) {
                    if ((n = o.data.length) && i < n)
                        return !1;

                    Re.currentNode = o;
                }
                else
                    Re.currentNode = Se(o, i);

                return Re.root = Be(e, t), !Re.nextNode();
            },
            Pe = function (e, t) {
                var n, o = Ae(e, t), i = Be(e, t);

                o && i && (n = o.parentNode,
                    e.setStart(n, ue.call(n.childNodes, o)),
                    n = i.parentNode,
                    e.setEnd(n, ue.call(n.childNodes, i) + 1));
            },
            Ie = {
                8: "backspace",
                9: "tab",
                13: "enter",
                32: "space",
                33: "pageup",
                34: "pagedown",
                37: "left",
                39: "right",
                46: "delete",
                219: "[", 221: "]"
            },
            we = function (e) {
                var t = e.keyCode, n = Ie[t], o = "", i = this.getSelection();

                e.defaultPrevented || (n || (n = String.fromCharCode(t).toLowerCase(), /^[A-Za-z0-9]$/.test(n) || (n = "")),
                    te && 46 === e.which && (n = "."),
                    111 < t && t < 124 && (n = "f" + (t - 111)),
                    "backspace" !== n && "delete" !== n && (e.altKey && (o += "alt-"), e.ctrlKey && (o += "ctrl-"), e.metaKey && (o += "meta-")),
                    e.shiftKey && (o += "shift-"),
                    n = o + n,
                    this._keyHandlers[n] ? this._keyHandlers[n](this, e, i) : 1 !== n.length || i.collapsed || (this.saveUndoState(i),
                        be(i, this._root),
                        this._ensureBottomLine(),
                        this.setSelection(i),
                        this._updatePath(i, !0)));
            },
            Fe = function (e) {
                return function (t, n) {
                    n.preventDefault(), t[e]();
                };
            },
            Me = function (e, t) {
                return t = t || null, function (n, o) {
                    o.preventDefault();

                    var i = n.getSelection();

                    n.hasFormat(e, null, i) ? n.changeFormat(null, { tag: e }, i) : n.changeFormat({ tag: e }, t, i);
                };
            },
            He = function (e, t) {
                try {
                    t || (t = e.getSelection());
                    var n, o = t.startContainer;

                    for (o.nodeType === F && (o = o.parentNode), n = o; a(n) && (!n.textContent || n.textContent === Z);)
                        o = n, n = o.parentNode;

                    o !== n && (t.setStart(n, ue.call(n.childNodes, o)),
                        t.collapse(!0),
                        n.removeChild(o),
                        s(n) || (n = c(n, e._root)), y(n, e._root), xe(t)),
                        o === e._root && (o = o.firstChild) && "BR" === o.nodeName && N(o),
                        e._ensureBottomLine(),
                        e.setSelection(t),
                        e._updatePath(t, !0);
                }
                catch (t) {
                    e.didError(t);
                }
            },
            We = {
                enter: function (e, t, n) {
                    var o, i, r, a = e._root;

                    if (t.preventDefault(),
                        e._recordUndoState(n),
                        yt(n.startContainer, a, e),
                        e._removeZWS(),
                        e._getRangeAndRemoveBookmark(n),
                        n.collapsed || be(n, a),
                        !(o = Ae(n, a)) || /^T[HD]$/.test(o.nodeName))
                        return i = g(n.endContainer, a, "A"),
                            i && (i = i.parentNode, Oe(n, i, i, a), n.collapse(!1)),
                            ye(n, e.createElement("BR")),
                            n.collapse(!1),
                            e.setSelection(n),
                            void e._updatePath(n, !0);

                    if ((i = g(o, a, "LI")) && (o = i), u(o)) {
                        if (g(o, a, "UL") || g(o, a, "OL"))
                            return e.decreaseListLevel(n);

                        if (g(o, a, "BLOCKQUOTE"))
                            return e.modifyBlocks(mt, n);
                    }

                    for (r = ft(e, o, n.startContainer, n.startOffset), ct(o), Ye(o), y(o, a); r.nodeType === w;) {
                        var s, d = r.firstChild;

                        if ("A" === r.nodeName && (!r.textContent || r.textContent === Z)) {
                            d = e._doc.createTextNode(""), C(r, d), r = d;

                            break;
                        }

                        for (; d && d.nodeType === F && !d.data && (s = d.nextSibling) && "BR" !== s.nodeName;)
                            N(d), d = s;

                        if (!d || "BR" === d.nodeName || d.nodeType === F && !te)
                            break;

                        r = d;
                    }

                    n = e._createRange(r, 0), e.setSelection(n), e._updatePath(n, !0);
                },
                backspace: function (e, t, n) {
                    var o = e._root;
                    if (e._removeZWS(), e.saveUndoState(n), n.collapsed)
                        if (De(n, o)) {
                            t.preventDefault();

                            var i, r = Ae(n, o);

                            if (!r)
                                return;

                            if (E(r.parentNode, o), i = c(r, o)) {
                                if (!i.isContentEditable)
                                    return void N(i);

                                for (x(i, r, n, o), r = i.parentNode; r !== o && !r.nextSibling;)
                                    r = r.parentNode;

                                r !== o && (r = r.nextSibling) && O(r, o), e.setSelection(n);
                            }
                            else if (r) {
                                if (g(r, o, "UL") || g(r, o, "OL"))
                                    return e.decreaseListLevel(n);

                                if (g(r, o, "BLOCKQUOTE"))
                                    return e.modifyBlocks(gt, n); e.setSelection(n), e._updatePath(n, !0);
                            }
                        }
                        else
                            e.setSelection(n),
                                setTimeout(function () { He(e); }, 0);
                    else
                        t.preventDefault(), be(n, o), He(e, n);
                },
                delete: function (e, t, n) {
                    var o, i, r, a, s, d, l = e._root;

                    if (e._removeZWS(), e.saveUndoState(n), n.collapsed)
                        if (Ue(n, l)) {
                            if (t.preventDefault(), !(o = Ae(n, l)))
                                return;

                            if (E(o.parentNode, l), i = h(o, l)) {
                                if (!i.isContentEditable)
                                    return void N(i);

                                for (x(o, i, n, l), i = o.parentNode; i !== l && !i.nextSibling;)
                                    i = i.parentNode; i !== l && (i = i.nextSibling) && O(i, l), e.setSelection(n), e._updatePath(n, !0);
                            }
                        }
                        else {
                            if (r = n.cloneRange(),
                                Oe(n, l, l, l),
                                a = n.endContainer,
                                s = n.endOffset,
                                a.nodeType === w && (d = a.childNodes[s]) && "IMG" === d.nodeName)
                                return t.preventDefault(),
                                    N(d),
                                    xe(n),
                                    void He(e, n);

                            e.setSelection(r),
                                setTimeout(function () { He(e); }, 0);
                        }
                    else
                        t.preventDefault(), be(n, l), He(e, n);
                },
                tab: function (e, t, n) {
                    var o, i, r = e._root;

                    if (e._removeZWS(), n.collapsed && De(n, r))
                        for (o = Ae(n, r); i === o.parentNode;) {
                            if ("UL" === i.nodeName || "OL" === i.nodeName) {
                                t.preventDefault(), e.increaseListLevel(n);
                                break;
                            }

                            o = i;
                        }
                },
                "shift-tab": function (e, t, n) {
                    var o, i = e._root;

                    e._removeZWS(),
                        n.collapsed && De(n, i) && (o = n.startContainer,
                            (g(o, i, "UL") || g(o, i, "OL")) && (t.preventDefault(), e.decreaseListLevel(n)));
                },
                space: function (e, t, n) {
                    var o, i;

                    e._recordUndoState(n),
                        yt(n.startContainer, e._root, e),
                        e._getRangeAndRemoveBookmark(n),
                        o = n.endContainer,
                        i = o.parentNode,
                        n.collapsed && "A" === i.nodeName && !o.nextSibling && n.endOffset === _(o) ? n.setStartAfter(i) : n.collapsed || (be(n, e._root),
                            e._ensureBottomLine(),
                            e.setSelection(n),
                            e._updatePath(n, !0)),
                        e.setSelection(n);
                },
                left: function (e) {
                    e._removeZWS();
                },
                right: function (e) {
                    e._removeZWS();
                }
            };

        Y && J && (We["meta-left"] = function (e, t) {
            t.preventDefault(); var n = lt(e);

            n && n.modify && n.modify("move", "backward", "lineboundary");
        },
            We["meta-right"] = function (e, t) {
                t.preventDefault();

                var n = lt(e);
                n && n.modify && n.modify("move", "forward", "lineboundary");
            }),
            Y || (We.pageup = function (e) {
                e.moveCursorToStart();
            },
                We.pagedown = function (e) {
                    e.moveCursorToEnd();
                }),
            We[re + "b"] = Me("B"),
            We[re + "i"] = Me("I"),
            We[re + "u"] = Me("U"),
            We[re + "shift-7"] = Me("S"),
            We[re + "shift-5"] = Me("SUB", { tag: "SUP" }),
            We[re + "shift-6"] = Me("SUP", { tag: "SUB" }),
            We[re + "shift-8"] = Fe("makeUnorderedList"),
            We[re + "shift-9"] = Fe("makeOrderedList"),
            We[re + "["] = Fe("decreaseQuoteLevel"),
            We[re + "]"] = Fe("increaseQuoteLevel"),
            We[re + "y"] = Fe("redo"),
            We[re + "z"] = Fe("undo"),
            We[re + "shift-z"] = Fe("redo");

        var ze = {
            1: 10,
            2: 13,
            3: 16,
            4: 18,
            5: 24,
            6: 32,
            7: 48
        },
            qe = {
                backgroundColor: {
                    regexp: he, replace: function (e, t) {
                        return T(e, "SPAN", { class: z, style: "background-color:" + t });
                    }
                },
                color: {
                    regexp: he, replace: function (e, t) {
                        return T(e, "SPAN", { class: q, style: "color:" + t });
                    }
                },
                fontWeight: {
                    regexp: /^bold|^700/i, replace: function (e) {
                        return T(e, "B");
                    }
                },
                fontStyle: {
                    regexp: /^italic/i, replace: function (e) {
                        return T(e, "I");
                    }
                },
                fontFamily: {
                    regexp: he, replace: function (e, t) {
                        return T(e, "SPAN", { class: K, style: "font-family:" + t });
                    }
                },
                fontSize: {
                    regexp: he, replace: function (e, t) {
                        return T(e, "SPAN", { class: G, style: "font-size:" + t });
                    }
                },
                textDecoration: {
                    regexp: /^underline/i, replace: function (e) {
                        return T(e, "U");
                    }
                }
            },
            Ke = function (e) {
                return function (t, n) {
                    var o = T(t.ownerDocument, e);

                    return n.replaceChild(o, t), o.appendChild(S(t)), o;
                };
            },
            Ge = function (e, t) {
                var n, o, i, r, a, s, d = e.style, l = e.ownerDocument;

                for (n in qe) o = qe[n],
                    (i = d[n]) && o.regexp.test(i) && (s = o.replace(l, i),
                        a || (a = s),
                        r && r.appendChild(s),
                        r = s, e.style[n] = "");

                return a && (r.appendChild(S(e)), "SPAN" === e.nodeName ? t.replaceChild(a, e) : e.appendChild(a)), r || e;
            },
            Ze = {
                P: Ge,
                SPAN: Ge,
                STRONG: Ke("B"),
                EM: Ke("I"),
                INS: Ke("U"),
                STRIKE: Ke("S"),
                FONT: function (e, t) {
                    var n, o, i, r, a, s = e.face, d = e.size, l = e.color, c = e.ownerDocument;

                    return s && (n = T(c, "SPAN", {
                        class: K,
                        style: "font-family:" + s
                    }), a = n, r = n),
                        d && (o = T(c, "SPAN", {
                            class: G,
                            style: "font-size:" + ze[d] + "px"
                        }),
                            a || (a = o), r && r.appendChild(o), r = o),
                        l && /^#?([\dA-F]{3}){1,2}$/i.test(l) && ("#" !== l.charAt(0) && (l = "#" + l),
                            i = T(c, "SPAN", {
                                class: q,
                                style: "color:" + l
                            }),
                            a || (a = i),
                            r && r.appendChild(i), r = i),
                        a || (a = r = T(c, "SPAN")),
                        t.replaceChild(a, e),
                        r.appendChild(S(e)),
                        r;
                },
                TT: function (e, t) {
                    var n = T(e.ownerDocument, "SPAN", {
                        class: K,
                        style: 'font-family:menlo,consolas,"courier new",monospace'
                    });

                    return t.replaceChild(n, e), n.appendChild(S(e)), n;
                }
            },
            je = /^(?:A(?:DDRESS|RTICLE|SIDE|UDIO)|BLOCKQUOTE|CAPTION|D(?:[DLT]|IV)|F(?:IGURE|IGCAPTION|OOTER)|H[1-6]|HEADER|L(?:ABEL|EGEND|I)|O(?:L|UTPUT)|P(?:RE)?|SECTION|T(?:ABLE|BODY|D|FOOT|H|HEAD|R)|COL(?:GROUP)?|UL)$/,
            $e = /^(?:HEAD|META|STYLE)/, Qe = new n(null, 4 | W, function () { return !0; }),
            Ve = function e(t, n) {
                var o, i, r, s, d, l, c, h, u, f, p, g, m = t.childNodes;

                for (o = t; a(o);)
                    o = o.parentNode;

                for (Qe.root = o, i = 0, r = m.length; i < r; i += 1)
                    if (s = m[i], d = s.nodeName, l = s.nodeType, c = Ze[d], l === w) {
                        if (h = s.childNodes.length, c)
                            s = c(s, t);
                        else {
                            if ($e.test(d)) {
                                t.removeChild(s), i -= 1, r -= 1;

                                continue;
                            }
                            if (!je.test(d) && !a(s)) {
                                i -= 1, r += h - 1, t.replaceChild(S(s), s);

                                continue;
                            }
                        }

                        h && e(s, n || "PRE" === d);
                    }
                    else {
                        if (l === F) {
                            if (p = s.data, u = !he.test(p.charAt(0)), f = !he.test(p.charAt(p.length - 1)), n || !u && !f)
                                continue;

                            if (u) {
                                for (Qe.currentNode = s; (g = Qe.previousPONode()) && !("IMG" === (d = g.nodeName) || "#text" === d && he.test(g.data));)
                                    if (!a(g)) {
                                        g = null;

                                        break;
                                    }

                                p = p.replace(/^[ \t\r\n]+/g, g ? " " : "");
                            }

                            if (f) {
                                for (Qe.currentNode = s; (g = Qe.nextNode()) && !("IMG" === d || "#text" === d && he.test(g.data));)
                                    if (!a(g)) {
                                        g = null;

                                        break;
                                    }

                                p = p.replace(/[ \t\r\n]+$/g, g ? " " : "");
                            }

                            if (p) {
                                s.data = p;

                                continue;
                            }
                        }

                        t.removeChild(s), i -= 1, r -= 1;
                    }

                return t;
            },
            Ye = function e(t) {
                for (var n, o = t.childNodes, r = o.length; r--;)
                    n = o[r],
                        n.nodeType !== w || i(n) ? n.nodeType !== F || n.data || t.removeChild(n) : (e(n),
                            a(n) && !n.firstChild && t.removeChild(n));
            },
            Xe = function (e) {
                return e.nodeType === w ? "BR" === e.nodeName : he.test(e.data);
            },
            Je = function (e, t) {
                for (var o, i = e.parentNode; a(i);)
                    i = i.parentNode;

                return o = new n(i, 4 | W, Xe), o.currentNode = e, !!o.nextNode() || t && !o.previousNode();
            },
            et = function (e, t, n) {
                var o, i, r, s = e.querySelectorAll("BR"), d = [], l = s.length;

                for (o = 0; o < l; o += 1)
                    d[o] = Je(s[o], n);

                for (; l--;)
                    i = s[l], (r = i.parentNode) && (d[l] ? a(r) || E(r, t) : N(i));
            },
            tt = function (e, t, n) {
                var o, i, r = t.ownerDocument.body;

                et(t, n, !0),
                    t.setAttribute("style", "position:fixed;overflow:hidden;bottom:100%;right:100%;"),
                    r.appendChild(t),
                    o = t.innerHTML,
                    i = t.innerText || t.textContent,
                    X && (i = i.replace(/\r?\n/g, "\r\n")),
                    e.setData("text/html", o),
                    e.setData("text/plain", i),
                    r.removeChild(t);
            },
            nt = function (e) {
                var t, n, o, i, r, a, s, d = e.clipboardData, l = this.getSelection(), c = this._root, h = this;

                if (l.collapsed)
                    return void e.preventDefault();

                if (this.saveUndoState(l), ne || V || !d) setTimeout(function () {
                    try {
                        h._ensureBottomLine();
                    }
                    catch (e) {
                        h.didError(e);
                    }
                }, 0);
                else {
                    for (t = Ae(l, c),
                        n = Be(l, c),
                        o = t === n && t || c,
                        i = be(l, c),
                        r = l.commonAncestorContainer,
                        r.nodeType === F && (r = r.parentNode);
                        r && r !== o;)
                        a = r.cloneNode(!1),
                            a.appendChild(i),
                            i = a,
                            r = r.parentNode;

                    s = this.createElement("div"), s.appendChild(i), tt(d, s, c), e.preventDefault();
                }

                this.setSelection(l);
            },
            ot = function (e) {
                var t, n, o, i, r, a, s, d = e.clipboardData, l = this.getSelection(), c = this._root;

                if (!ne && !V && d) {
                    for (t = Ae(l, c),
                        n = Be(l, c),
                        o = t === n && t || c,
                        l = l.cloneRange(),
                        xe(l),
                        Oe(l, o, o, c),
                        i = l.cloneContents(),
                        r = l.commonAncestorContainer,
                        r.nodeType === F && (r = r.parentNode); r && r !== o;)a = r.cloneNode(!1),
                            a.appendChild(i),
                            i = a,
                            r = r.parentNode;

                    s = this.createElement("div"), s.appendChild(i), tt(d, s, c), e.preventDefault();
                }
            },
            it = function (e) {
                var t,
                    n,
                    o,
                    i,
                    r,
                    a = e.clipboardData,
                    s = a && a.items,
                    d = this.isShiftDown,
                    l = !1, c = !1,
                    h = null,
                    u = this;

                if (ne && s) {
                    for (t = s.length; t--;)!d && /^image\/.*/.test(s[t].type) && (c = !0);

                    c || (s = null);
                }

                if (s) {
                    for (e.preventDefault(), t = s.length; t--;) {
                        if (n = s[t], o = n.type, !d && "text/html" === o)
                            return void n.getAsString(function (e) { u.insertHTML(e, !0); });

                        "text/plain" === o && (h = n), !d && /^image\/.*/.test(o) && (c = !0);
                    }

                    return void (c ? (this.fireEvent("dragover", {
                        dataTransfer: a, preventDefault: function () { l = !0; }
                    }),
                        l && this.fireEvent("drop", { dataTransfer: a })) : h && h.getAsString(function (e) { u.insertPlainText(e, !0); }));
                }

                if (i = a && a.types, !ne && i && (ue.call(i, "text/html") > -1 || !J && ue.call(i, "text/plain") > -1 && ue.call(i, "text/rtf") < 0))
                    return e.preventDefault(),
                        void (!d && (r = a.getData("text/html")) ? this.insertHTML(r, !0) : ((r = a.getData("text/plain")) || (r = a.getData("text/uri-list"))) && this.insertPlainText(r, !0));

                this._awaitingPaste = !0;

                var f = this._doc.body,
                    p = this.getSelection(),
                    g = p.startContainer,
                    m = p.startOffset,
                    v = p.endContainer,
                    _ = p.endOffset,
                    C = this.createElement("DIV", {
                        contenteditable: "true",
                        style: "position:fixed; overflow:hidden; top:0; right:100%; width:1px; height:1px;"
                    });

                f.appendChild(C),
                    p.selectNodeContents(C),
                    this.setSelection(p),
                    setTimeout(function () {
                        try {
                            u._awaitingPaste = !1;

                            for (var e, t, n = "", o = C; C === o;)
                                o = C.nextSibling, N(C),
                                    e = C.firstChild,
                                    e && e === C.lastChild && "DIV" === e.nodeName && (C = e),
                                    n += C.innerHTML;

                            t = u._createRange(g, m, v, _),
                                u.setSelection(t),
                                n && u.insertHTML(n, !0);
                        }
                        catch (e) {
                            u.didError(e);
                        }
                    }, 0);
            },
            rt = function (e) {
                for (var t = e.dataTransfer.types, n = t.length, o = !1, i = !1; n--;)
                    switch (t[n]) {
                        case "text/plain": o = !0;
                            break;
                        case "text/html": i = !0; break;
                        default: return;
                    }

                (i || o) && this.saveUndoState();
            },
            at = R.prototype, st = function (e, t, n) {
                var o = n._doc, i = e ? DOMPurify.sanitize(e, {
                    ALLOW_UNKNOWN_PROTOCOLS: !0, WHOLE_DOCUMENT: !1, RETURN_DOM: !0, RETURN_DOM_FRAGMENT: !0
                }) : null; return i ? o.importNode(i, !0) : o.createDocumentFragment();
            };
        at.setConfig = function (e) {
            return e = B({
                blockTag: "DIV", blockAttributes: null, tagAttributes: {
                    blockquote: null, ul: null, ol: null, li: null, a: null
                },
                leafNodeNames: ge, undo: { documentSizeThreshold: -1, undoLimit: -1 },
                isInsertedHTMLSanitized: !0,
                isSetHTMLSanitized: !0,
                sanitizeToDOMFragment: "undefined" !== typeof DOMPurify && DOMPurify.isSupported ? st : null
            }, e, !0),
                e.blockTag = e.blockTag.toUpperCase(),
                this._config = e,
                this;
        },
            at.createElement = function (e, t, n) {
                return T(this._doc, e, t, n);
            },
            at.createDefaultBlock = function (e) {
                var t = this._config;

                return y(this.createElement(t.blockTag, t.blockAttributes, e), this._root);
            },
            at.didError = function (e) {
                console.log(e);
            }, at.getDocument = function () {
                return this._doc;
            }, at.getRoot = function () {
                return this._root;
            }, at.modifyDocument = function (e) {
                var t = this._mutation;

                t && (t.takeRecords().length && this._docWasChanged(),
                    t.disconnect()),
                    this._ignoreAllChanges = !0, e(),
                    this._ignoreAllChanges = !1,
                    t && (t.observe(this._root, {
                        childList: !0, attributes: !0, characterData: !0, subtree: !0
                    }), this._ignoreChange = !1);
            };

        var dt = {
            pathChange: 1,
            select: 1,
            input: 1,
            undoStateChange: 1
        };

        at.fireEvent = function (e, t) {
            var n, o, i, r = this._events[e];

            if (/^(?:focus|blur)/.test(e))
                if (n = this._root === this._doc.activeElement, "focus" === e) {
                    if (!n || this._isFocused)
                        return this;

                    this._isFocused = !0;
                }
                else {
                    if (n || !this._isFocused)
                        return this;

                    this._isFocused = !1;
                }
            if (r)
                for (t || (t = {}), t.type !== e && (t.type = e), r = r.slice(), o = r.length; o--;) {
                    i = r[o];

                    try {
                        i.handleEvent ? i.handleEvent(t) : i.call(this, t);
                    }
                    catch (t) {
                        t.details = "Squire: fireEvent error. Event type: " + e, this.didError(t);
                    }
                }

            return this;
        },
            at.destroy = function () {
                var e, t = this._events; for (e in t) this.removeEventListener(e);

                this._mutation && this._mutation.disconnect(),
                    delete this._root.__squire__,
                    this._undoIndex = -1,
                    this._undoStack = [],
                    this._undoStackLength = 0;
            },
            at.handleEvent = function (e) {
                this.fireEvent(e.type, e);
            },
            at.addEventListener = function (e, t) {
                var n = this._events[e],
                    o = this._root;

                return t ? (n || (n = this._events[e] = [],
                    dt[e] || ("selectionchange" === e && (o = this._doc),
                        o.addEventListener(e, this, !0))),
                    n.push(t),
                    this) : (this.didError({ name: "Squire: addEventListener with null or undefined fn", message: "Event type: " + e }),
                        this);
            },
            at.removeEventListener = function (e, t) {
                var n, o = this._events[e],
                    i = this._root;

                if (o) {
                    if (t)
                        for (n = o.length; n--;)
                            o[n] === t && o.splice(n, 1);
                    else
                        o.length = 0; o.length || (delete this._events[e],
                            dt[e] || ("selectionchange" === e && (i = this._doc),
                                i.removeEventListener(e, this, !0)));
                }

                return this;
            },
            at._createRange = function (e, t, n, o) {
                if (e instanceof this._win.Range)
                    return e.cloneRange();

                var i = this._doc.createRange();

                return i.setStart(e, t), n ? i.setEnd(n, o) : i.setEnd(e, t), i;
            },
            at.getCursorPosition = function (e) {
                if (!e && !(e = this.getSelection()) || !e.getBoundingClientRect)
                    return null;

                var t, n, o = e.getBoundingClientRect();

                return o && !o.top && (this._ignoreChange = !0,
                    t = this._doc.createElement("SPAN"),
                    t.textContent = Z, ye(e, t),
                    o = t.getBoundingClientRect(),
                    n = t.parentNode,
                    n.removeChild(t),
                    L(n, e)),
                    o;
            },
            at._moveCursorTo = function (e) {
                var t = this._root,
                    n = this._createRange(t, e ? 0 : t.childNodes.length);

                return xe(n), this.setSelection(n), this;
            },
            at.moveCursorToStart = function () {
                return this._moveCursorTo(!0);
            },
            at.moveCursorToEnd = function () {
                return this._moveCursorTo(!1);
            };
        var lt = function (e) {
            return e._win.getSelection() || null;
        };
        at.setSelection = function (e) {
            if (e)
                if (this._lastSelection = e, this._isFocused)
                    if (Q && !this._restoreSelection)
                        D.call(this), this.blur(), this.focus();
                    else {
                        V && this._win.focus();
                        var t = lt(this);
                        t && (t.removeAllRanges(), t.addRange(e));
                    }
                else
                    D.call(this);

            return this;
        },
            at.getSelection = function () {
                var e,
                    t,
                    n,
                    o,
                    r = lt(this),
                    a = this._root;

                return this._isFocused && r && r.rangeCount && (e = r.getRangeAt(0).cloneRange(),
                    t = e.startContainer,
                    n = e.endContainer,
                    t && i(t) && e.setStartBefore(t),
                    n && i(n) && e.setEndBefore(n)),
                    e && m(a, e.commonAncestorContainer) ? this._lastSelection = e : (e = this._lastSelection, o = e.commonAncestorContainer,
                        m(o.ownerDocument, o) || (e = null)), e || (e = this._createRange(a.firstChild, 0)),
                    e;
            },
            at.getSelectedText = function () {
                var e = this.getSelection();

                if (!e || e.collapsed)
                    return "";

                var t,
                    o = new n(e.commonAncestorContainer, 4 | W, function (t) { return Le(e, t, !0); }),
                    i = e.startContainer, r = e.endContainer, s = o.currentNode = i,
                    d = "",
                    l = !1;

                for (o.filter(s) || (s = o.nextNode()); s;)
                    s.nodeType === F ? (t = s.data) && /\S/.test(t) && (s === r && (t = t.slice(0, e.endOffset)),
                        s === i && (t = t.slice(e.startOffset)), d += t, l = !0) : ("BR" === s.nodeName || l && !a(s)) && (d += "\n", l = !1),
                        s = o.nextNode();

                return d;
            },
            at.getPath = function () {
                return this._path;
            };
        var ct = function (e, t) {
            for (var o, i, r, s = new n(e, 4, function () { return !0; }, !1); i === s.nextNode();)
                for (; (r = i.data.indexOf(Z)) > -1 && (!t || i.parentNode !== t);) {
                    if (1 === i.length) {
                        do {
                            o = i.parentNode, o.removeChild(i), i = o, s.currentNode = o;
                        } while (a(i) && !_(i));

                        break;
                    }

                    i.deleteData(r, 1);
                }
        };
        at._didAddZWS = function () {
            this._hasZWS = !0;
        },
            at._removeZWS = function () {
                this._hasZWS && (ct(this._root), this._hasZWS = !1);
            },
            at._updatePath = function (e, t) {
                if (e) {
                    var n, o = e.startContainer, i = e.endContainer;

                    (t || o !== this._lastAnchorNode || i !== this._lastFocusNode) && (this._lastAnchorNode = o, this._lastFocusNode = i, n = o && i ? o === i ? v(i, this._root) : "(selection)" : "", this._path !== n && (this._path = n, this.fireEvent("pathChange", { path: n }))), this.fireEvent(e.collapsed ? "cursor" : "select", { range: e });
                }
            },
            at._updatePathOnEvent = function (e) {
                var t = this;

                t._isFocused && !t._willUpdatePath && (t._willUpdatePath = !0, setTimeout(function () {
                    t._willUpdatePath = !1, t._updatePath(t.getSelection());
                }, 0));
            },
            at.focus = function () {
                return this._root.focus(), ie && this.fireEvent("focus"), this;
            },
            at.blur = function () {
                return this._root.blur(), ie && this.fireEvent("blur"), this;
            };

        var ht = "squire-selection-end";
        at._saveRangeToBookmark = function (e) {
            var t, n = this.createElement("INPUT", {
                id: "squire-selection-start", type: "hidden"
            }), o = this.createElement("INPUT", {
                id: ht,
                type: "hidden"
            });
            ye(e, n),
                e.collapse(!1),
                ye(e, o),
                2 & n.compareDocumentPosition(o) && (n.id = ht, o.id = "squire-selection-start", t = n, n = o, o = t),
                e.setStartAfter(n), e.setEndBefore(o);
        },
            at._getRangeAndRemoveBookmark = function (e) {
                var t = this._root, n = t.querySelector("#squire-selection-start"), o = t.querySelector("#" + ht);

                if (n && o) {
                    var i = n.parentNode,
                        r = o.parentNode,
                        a = ue.call(i.childNodes, n),
                        s = ue.call(r.childNodes, o);

                    i === r && (s -= 1),
                        N(n),
                        N(o),
                        e || (e = this._doc.createRange()),
                        e.setStart(i, a),
                        e.setEnd(r, s),
                        L(i, e),
                        i !== r && L(r, e),
                        e.collapsed && (i = e.startContainer,
                            i.nodeType === F && (r = i.childNodes[e.startOffset],
                                r && r.nodeType === F || (r = i.childNodes[e.startOffset - 1]),
                                r && r.nodeType === F && (e.setStart(r, 0), e.collapse(!0))));
                }

                return e || null;
            },
            at._keyUpDetectChange = function (e) {
                var t = e.keyCode; e.ctrlKey || e.metaKey || e.altKey || !(t < 16 || t > 20) || !(t < 33 || t > 45) || this._docWasChanged();
            },
            at._docWasChanged = function () {
                if (ce && (Ce = new WeakMap), !this._ignoreAllChanges) {
                    if (le && this._ignoreChange)
                        return void (this._ignoreChange = !1);

                    this._isInUndoState && (this._isInUndoState = !1,
                        this.fireEvent("undoStateChange", { canUndo: !0, canRedo: !1 })), this.fireEvent("input");
                }
            },
            at._recordUndoState = function (e, t) {
                if (!this._isInUndoState || t) {
                    var n, o = this._undoIndex,
                        i = this._undoStack,
                        r = this._config.undo,
                        a = r.documentSizeThreshold,
                        s = r.undoLimit; t || (o += 1),
                            o < this._undoStackLength && (i.length = this._undoStackLength = o),
                            e && this._saveRangeToBookmark(e),
                            n = this._getHTML(),
                            a > -1 && 2 * n.length > a && s > -1 && o > s && (i.splice(0, o - s),
                                o = s, this._undoStackLength = s),
                            i[o] = n,
                            this._undoIndex = o,
                            this._undoStackLength += 1,
                            this._isInUndoState = !0;
                }
            },
            at.saveUndoState = function (e) {
                return e === t && (e = this.getSelection()),
                    this._recordUndoState(e, this._isInUndoState),
                    this._getRangeAndRemoveBookmark(e),
                    this;
            },
            at.undo = function () {
                if (0 !== this._undoIndex || !this._isInUndoState) {
                    this._recordUndoState(this.getSelection(), !1), this._undoIndex -= 1, this._setHTML(this._undoStack[this._undoIndex]);

                    var e = this._getRangeAndRemoveBookmark();
                    e && this.setSelection(e),
                        this._isInUndoState = !0,
                        this.fireEvent("undoStateChange", {
                            canUndo: 0 !== this._undoIndex, canRedo: !0
                        }), this.fireEvent("input");
                }

                return this;
            },
            at.redo = function () {
                var e = this._undoIndex, t = this._undoStackLength;

                if (e + 1 < t && this._isInUndoState) {
                    this._undoIndex += 1, this._setHTML(this._undoStack[this._undoIndex]);

                    var n = this._getRangeAndRemoveBookmark();
                    n && this.setSelection(n), this.fireEvent("undoStateChange", { canUndo: !0, canRedo: e + 2 < t }), this.fireEvent("input");
                }

                return this;
            },
            at.hasFormat = function (e, t, o) {
                if (e = e.toUpperCase(), t || (t = {}), !o && !(o = this.getSelection()))
                    return !1;

                !o.collapsed && o.startContainer.nodeType === F && o.startOffset === o.startContainer.length && o.startContainer.nextSibling && o.setStartBefore(o.startContainer.nextSibling),
                    !o.collapsed && o.endContainer.nodeType === F && 0 === o.endOffset && o.endContainer.previousSibling && o.setEndAfter(o.endContainer.previousSibling);

                var i, r, a = this._root, s = o.commonAncestorContainer;

                if (g(s, a, e, t))
                    return !0;

                if (s.nodeType === F)
                    return !1;

                i = new n(s, 4, function (e) {
                    return Le(o, e, !0);
                }, !1);

                for (var d = !1; r === i.nextNode();) {
                    if (!g(r, a, e, t)) return !1; d = !0;
                }

                return d;
            },
            at.getFontInfo = function (e) {
                var n, o, i, r = {
                    color: t,
                    backgroundColor: t,
                    family: t,
                    size: t
                }, a = 0;

                if (!e && !(e = this.getSelection()))
                    return r;

                if (n = e.commonAncestorContainer, e.collapsed || n.nodeType === F)
                    for (n.nodeType === F && (n = n.parentNode); a < 4 && n;)
                        (o = n.style) && (!r.color && (i = o.color) && (r.color = i, a += 1),
                            !r.backgroundColor && (i = o.backgroundColor) && (r.backgroundColor = i, a += 1),
                            !r.family && (i = o.fontFamily) && (r.family = i, a += 1),
                            !r.size && (i = o.fontSize) && (r.size = i, a += 1)),
                            n = n.parentNode;

                return r;
            },
            at._addFormat = function (e, t, o) {
                var i, r, s, d, l, c, h, u, f = this._root;

                if (o.collapsed) {
                    for (i = y(this.createElement(e, t), f), ye(o, i), o.setStart(i.firstChild, i.firstChild.length), o.collapse(!0), u = i; a(u);)
                        u = u.parentNode; ct(u, i);
                }
                else {
                    if (r = new n(o.commonAncestorContainer,
                        4 | W,
                        function (e) {
                            return (e.nodeType === F || "BR" === e.nodeName || "IMG" === e.nodeName) && Le(o, e, !0);
                        }, !1),
                        s = o.startContainer,
                        l = o.startOffset,
                        d = o.endContainer,
                        c = o.endOffset,
                        r.currentNode = s,
                        r.filter(s) || (s = r.nextNode(), l = 0), !s) return o;

                    do {
                        h = r.currentNode, !g(h, f, e, t) && (h === d && h.length > c && h.splitText(c), h === s && l && (h = h.splitText(l),
                            d === s && (d = h, c -= l), s = h, l = 0), i = this.createElement(e, t), C(h, i), i.appendChild(h));
                    } while (r.nextNode());

                    d.nodeType !== F && (h.nodeType === F ? (d = h, c = h.length) : (d = h.parentNode, c = 1)), o = this._createRange(s, l, d, c);
                }

                return o;
            },
            at._removeFormat = function (e, t, n, o) {
                this._saveRangeToBookmark(n);

                var i, r = this._doc; n.collapsed && (se ? (i = r.createTextNode(Z), this._didAddZWS()) : i = r.createTextNode(""), ye(n, i));

                for (var s = n.commonAncestorContainer; a(s);)
                    s = s.parentNode; var d = n.startContainer,
                        l = n.startOffset,
                        c = n.endContainer,
                        h = n.endOffset,
                        u = [],
                        f = function (e, t) {
                            if (!Le(n, e, !1)) {
                                var o, i, r = e.nodeType === F;

                                if (!Le(n, e, !0))
                                    return void ("INPUT" === e.nodeName || r && !e.data || u.push([t, e]));

                                if (r)
                                    e === c && h !== e.length && u.push([t, e.splitText(h)]),
                                        e === d && l && (e.splitText(l), u.push([t, e]));
                                else
                                    for (o = e.firstChild; o; o = i)
                                        i = o.nextSibling, f(o, t);
                            }
                        },
                        g = Array.prototype.filter.call(s.getElementsByTagName(e),
                            function (o) {
                                return Le(n, o, !0) && p(o, e, t);
                            }); return o || g.forEach(function (e) {
                                f(e, e);
                            }), u.forEach(function (e) {
                                var t = e[0].cloneNode(!1), n = e[1]; C(n, t), t.appendChild(n);
                            }),
                                g.forEach(function (e) {
                                    C(e, S(e));
                                }), this._getRangeAndRemoveBookmark(n), i && n.collapse(!1), L(s, n), n;
            },
            at.changeFormat = function (e, t, n, o) {
                return n || (n = this.getSelection()) ? (this.saveUndoState(n), t && (n = this._removeFormat(t.tag.toUpperCase(), t.attributes || {}, n, o)),
                    e && (n = this._addFormat(e.tag.toUpperCase(), e.attributes || {}, n)),
                    this.setSelection(n), this._updatePath(n, !0),
                    le || this._docWasChanged(), this) : this;
            };

        var ut = { DT: "DD", DD: "DT", LI: "LI", PRE: "PRE" },
            ft = function (e, t, n, o) {
                var i = ut[t.nodeName],
                    r = null, a = b(n, o, t.parentNode, e._root),
                    s = e._config; return i || (i = s.blockTag, r = s.blockAttributes),
                        p(a, i, r) || (t = T(a.ownerDocument, i, r),
                            a.dir && (t.dir = a.dir),
                            C(a, t),
                            t.appendChild(S(a)), a = t),
                        a;
            };
        at.forEachBlock = function (e, t, n) {
            if (!n && !(n = this.getSelection()))
                return this;

            t && this.saveUndoState(n);
            var o = this._root, i = Ae(n, o), r = Be(n, o);

            if (i && r)
                do {
                    if (e(i) || i === r)
                        break;
                } while (i === h(i, o));

            return t && (this.setSelection(n), this._updatePath(n, !0), le || this._docWasChanged()), this;
        },
            at.modifyBlocks = function (e, t) {
                if (!t && !(t = this.getSelection()))
                    return this;

                this._recordUndoState(t, this._isInUndoState);
                var n, o = this._root;

                return Pe(t, o),
                    Oe(t, o, o, o),
                    n = Ee(t, o, o),
                    ye(t, e.call(this, n)),
                    t.endOffset < t.endContainer.childNodes.length && O(t.endContainer.childNodes[t.endOffset], o),
                    O(t.startContainer.childNodes[t.startOffset], o),
                    this._getRangeAndRemoveBookmark(t),
                    this.setSelection(t),
                    this._updatePath(t, !0),
                    le || this._docWasChanged(),
                    this;
            };

        var pt = function (e) {
            return this.createElement("BLOCKQUOTE", this._config.tagAttributes.blockquote, [e]);
        },
            gt = function (e) {
                var t = this._root, n = e.querySelectorAll("blockquote");

                return Array.prototype.filter.call(n, function (e) {
                    return !g(e.parentNode, t, "BLOCKQUOTE");
                }).forEach(function (e) { C(e, S(e)); }), e;
            },
            mt = function () {
                return this.createDefaultBlock([this.createElement("INPUT", {
                    id: "squire-selection-start",
                    type: "hidden"
                }),
                this.createElement("INPUT", { id: ht, type: "hidden" })]);
            },
            vt = function (e, t, n) {
                for (var o, i, r, a, s = l(t, e._root), d = e._config.tagAttributes, c = d[n.toLowerCase()], h = d.li; o === s.nextNode();)
                    "LI" === o.parentNode.nodeName && (o = o.parentNode, s.currentNode = o.lastChild),
                        "LI" !== o.nodeName ? (a = e.createElement("LI", h),
                            o.dir && (a.dir = o.dir),
                            (r = o.previousSibling) && r.nodeName === n ? (r.appendChild(a), N(o)) : C(o, e.createElement(n, c, [a])),
                            a.appendChild(S(o)), s.currentNode = a) : (o = o.parentNode, (i = o.nodeName) !== n && /^[OU]L$/.test(i) && C(o, e.createElement(n, c, [S(o)])));
            },
            _t = function (e) {
                return vt(this, e, "UL"), e;
            },
            Nt = function (e) {
                return vt(this, e, "OL"), e;
            },
            Ct = function (e) {
                var t, n, o, i, r, a = e.querySelectorAll("UL, OL"),
                    d = e.querySelectorAll("LI"), l = this._root;

                for (t = 0, n = a.length; t < n; t += 1)
                    o = a[t], i = S(o), E(i, l), C(o, i);

                for (t = 0, n = d.length; t < n; t += 1)
                    r = d[t], s(r) ? C(r, this.createDefaultBlock([S(r)])) : (E(r, l), C(r, S(r)));

                return e;
            },
            St = function (e, t) {
                for (var n = e.commonAncestorContainer, o = e.startContainer, i = e.endContainer; n && n !== t && !/^[OU]L$/.test(n.nodeName);)
                    n = n.parentNode; if (!n || n === t) return null;

                for (o === n && (o = o.childNodes[e.startOffset]), i === n && (i = i.childNodes[e.endOffset]); o && o.parentNode !== n;)
                    o = o.parentNode; for (; i && i.parentNode !== n;)i = i.parentNode;

                return [n, o, i];
            };

        at.increaseListLevel = function (e) {
            if (!e && !(e = this.getSelection()))
                return this.focus();

            var t = this._root, n = St(e, t);
            if (!n)
                return this.focus();

            var o = n[0], i = n[1], r = n[2];

            if (!i || i === o.firstChild)
                return this.focus();

            this._recordUndoState(e, this._isInUndoState);
            var a, s, d = o.nodeName, l = i.previousSibling;

            l.nodeName !== d && (a = this._config.tagAttributes[d.toLowerCase()], l = this.createElement(d, a), o.insertBefore(l, i));

            do {
                s = i === r ? null : i.nextSibling, l.appendChild(i);
            } while (i === s);

            return s = l.nextSibling,
                s && O(s, t),
                this._getRangeAndRemoveBookmark(e),
                this.setSelection(e),
                this._updatePath(e, !0),
                le || this._docWasChanged(), this.focus();
        },
            at.decreaseListLevel = function (e) {
                if (!e && !(e = this.getSelection()))
                    return this.focus();

                var t = this._root, n = St(e, t);

                if (!n)
                    return this.focus();

                var o = n[0], i = n[1], r = n[2];
                i || (i = o.firstChild), r || (r = o.lastChild),
                    this._recordUndoState(e, this._isInUndoState);

                var a, s = o.parentNode, d = r.nextSibling ? b(o, r.nextSibling, s, t) : o.nextSibling;

                if (s !== t && "LI" === s.nodeName) {
                    for (s = s.parentNode; d;)a = d.nextSibling, r.appendChild(d), d = a; d = o.parentNode.nextSibling;
                }

                var l = !/^[OU]L$/.test(s.nodeName);

                do {
                    a = i === r ? null : i.nextSibling, o.removeChild(i),
                        l && "LI" === i.nodeName && (i = this.createDefaultBlock([S(i)])),
                        s.insertBefore(i, d);
                } while (i === a);

                return o.firstChild || N(o), d && O(d, t),
                    this._getRangeAndRemoveBookmark(e),
                    this.setSelection(e),
                    this._updatePath(e, !0),
                    le || this._docWasChanged(),
                    this.focus();
            },
            at._ensureBottomLine = function () {
                var e = this._root,
                    t = e.lastElementChild; t && t.nodeName === this._config.blockTag && s(t) || e.appendChild(this.createDefaultBlock());
            },
            at.setKeyHandler = function (e, t) {
                return this._keyHandlers[e] = t, this;
            },
            at._getHTML = function () {
                return this._root.innerHTML;
            },
            at._setHTML = function (e) {
                var t = this._root, n = t;
                n.innerHTML = e;

                do {
                    y(n, t);
                } while (n === h(n, t));

                this._ignoreChange = !0;
            },
            at.getHTML = function (e) {
                var t, n, o, i, r, a, s = [];
                if (e && (a = this.getSelection()) && this._saveRangeToBookmark(a), ae)
                    for (t = this._root, n = t; n === h(n, t);)
                        n.textContent || n.querySelector("BR") || (o = this.createElement("BR"), n.appendChild(o), s.push(o));

                if (i = this._getHTML().replace(/\u200B/g, ""), ae)
                    for (r = s.length; r--;)N(s[r]);

                return a && this._getRangeAndRemoveBookmark(a), i;
            },
            at.setHTML = function (e) {
                var t, n, o, i = this._config,
                    r = i.isSetHTMLSanitized ? i.sanitizeToDOMFragment : null,
                    a = this._root; "function" === typeof r ? n = r(e, !1, this) : (t = this.createElement("DIV"),
                        t.innerHTML = e,
                        n = this._doc.createDocumentFragment(),
                        n.appendChild(S(t))),
                        Ve(n), et(n, a, !1), E(n, a);

                for (var s = n; s === h(s, a);)
                    y(s, a);

                for (this._ignoreChange = !0; o === a.lastChild;)
                    a.removeChild(o);

                a.appendChild(n), y(a, a), this._undoIndex = -1, this._undoStack.length = 0, this._undoStackLength = 0, this._isInUndoState = !1;

                var d = this._getRangeAndRemoveBookmark() || this._createRange(a.firstChild, 0);

                return this.saveUndoState(d), this._lastSelection = d, D.call(this), this._updatePath(d, !0), this;
            },
            at.insertElement = function (e, t) {
                if (t || (t = this.getSelection()), t.collapse(!0), a(e))
                    ye(t, e), t.setStartAfter(e);
                else {
                    for (var n, o, i = this._root, r = Ae(t, i) || i; r !== i && !r.nextSibling;)
                        r = r.parentNode;

                    r !== i && (n = r.parentNode, o = b(n, r.nextSibling, i, i)),
                        o ? i.insertBefore(e, o) : (i.appendChild(e),
                            o = this.createDefaultBlock(),
                            i.appendChild(o)),
                        t.setStart(o, 0),
                        t.setEnd(o, 0),
                        xe(t);
                }

                return this.focus(), this.setSelection(t), this._updatePath(t), le || this._docWasChanged(), this;
            },
            at.insertImage = function (e, t) {
                var n = this.createElement("IMG", B({ src: e }, t, !0)); return this.insertElement(n), n;
            };
        var Tt = /\b((?:(?:ht|f)tps?:\/\/|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,}\/)(?:[^\s()<>]+|\([^\s()<>]+\))+(?:\((?:[^\s()<>]+|(?:\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'".,<>?«»“”‘’]))|([\w\-.%+]+@(?:[\w\-]+\.)+[A-Z]{2,}\b)/i,
            yt = function (e, t, o) {
                for (var i, r, a, s, d, l, c, h = e.ownerDocument, u = new n(e, 4, function (e) {
                    return !g(e, t, "A");
                }, !1), f = o._config.tagAttributes.a; i === u.nextNode();)
                    for (r = i.data, a = i.parentNode; s === Tt.exec(r);)
                        d = s.index, l = d + s[0].length, d && (c = h.createTextNode(r.slice(0, d)),
                            a.insertBefore(c, i)),
                            c = o.createElement("A", B({ href: s[1] ? /^(?:ht|f)tps?:/.test(s[1]) ? s[1] : "http://" + s[1] : "mailto:" + s[2] }, f, !1)),
                            c.textContent = r.slice(d, l), a.insertBefore(c, i), i.data = r = r.slice(l);
            };
        at.insertHTML = function (e, t) {
            var n, o, i, r, a, s, d,
                l = this._config, c = l.isInsertedHTMLSanitized ? l.sanitizeToDOMFragment : null, u = this.getSelection(),
                f = this._doc; "function" === typeof c ? r = c(e, t, this) : (t && (n = e.indexOf("\x3c!--StartFragment--\x3e"),
                    o = e.lastIndexOf("\x3c!--EndFragment--\x3e"),
                    n > -1 && o > -1 && (e = e.slice(n + 20, o))),
                    /<\/td>((?!<\/tr>)[\s\S])*$/i.test(e) && (e = "<TR>" + e + "</TR>"),
                    /<\/tr>((?!<\/table>)[\s\S])*$/i.test(e) && (e = "<TABLE>" + e + "</TABLE>"),
                    i = this.createElement("DIV"), i.innerHTML = e, r = f.createDocumentFragment(), r.appendChild(S(i))), this.saveUndoState(u);

            try {
                for (a = this._root, s = r, d = {
                    fragment: r, preventDefault: function () {
                        this.defaultPrevented = !0;
                    }, defaultPrevented: !1
                }, yt(r, r, this), Ve(r), et(r, a, !1), Ye(r), r.normalize();

                    s === h(s, r);)y(s, a);

                t && this.fireEvent("willPaste", d),
                    d.defaultPrevented || (ke(u, d.fragment, a),
                        le || this._docWasChanged(),
                        u.collapse(!1),
                        this._ensureBottomLine()),
                    this.setSelection(u),
                    this._updatePath(u, !0),
                    t && this.focus();
            } catch (e) {
                this.didError(e);
            }

            return this;
        }; var Et = function (e) { return e.split("&").join("&amp;").split("<").join("&lt;").split(">").join("&gt;").split('"').join("&quot;"); }; at.insertPlainText = function (e, t) { var n, o, i, r, a = e.split("\n"), s = this._config, d = s.blockTag, l = s.blockAttributes, c = "</" + d + ">", h = "<" + d; for (n in l) h += " " + n + '="' + Et(l[n]) + '"'; for (h += ">", o = 0, i = a.length; o < i; o += 1)r = a[o], r = Et(r).replace(/ (?= )/g, "&nbsp;"), a[o] = h + (r || "<BR>") + c; return this.insertHTML(a.join(""), t); }; var bt = function (e, t, n) { return function () { return this[e](t, n), this.focus(); }; }; at.addStyles = function (e) { if (e) { var t = this._doc.documentElement.firstChild, n = this.createElement("STYLE", { type: "text/css" }); n.appendChild(this._doc.createTextNode(e)), t.appendChild(n); } return this; }, at.bold = bt("changeFormat", { tag: "B" }), at.italic = bt("changeFormat", { tag: "I" }), at.underline = bt("changeFormat", { tag: "U" }), at.strikethrough = bt("changeFormat", { tag: "S" }), at.subscript = bt("changeFormat", { tag: "SUB" }, { tag: "SUP" }), at.superscript = bt("changeFormat", { tag: "SUP" }, { tag: "SUB" }), at.removeBold = bt("changeFormat", null, { tag: "B" }), at.removeItalic = bt("changeFormat", null, { tag: "I" }), at.removeUnderline = bt("changeFormat", null, { tag: "U" }), at.removeStrikethrough = bt("changeFormat", null, { tag: "S" }), at.removeSubscript = bt("changeFormat", null, { tag: "SUB" }), at.removeSuperscript = bt("changeFormat", null, { tag: "SUP" }), at.makeLink = function (e, t) { var n = this.getSelection(); if (n.collapsed) { var o = e.indexOf(":") + 1; if (o) for (; "/" === e[o];)o += 1; ye(n, this._doc.createTextNode(e.slice(o))); } return t = B(B({ href: e }, t, !0), this._config.tagAttributes.a, !1), this.changeFormat({ tag: "A", attributes: t }, { tag: "A" }, n), this.focus(); }, at.removeLink = function () { return this.changeFormat(null, { tag: "A" }, this.getSelection(), !0), this.focus(); }, at.setFontFace = function (e) { return this.changeFormat(e ? { tag: "SPAN", attributes: { class: K, style: "font-family: " + e + ", sans-serif;" } } : null, { tag: "SPAN", attributes: { class: K } }), this.focus(); }, at.setFontSize = function (e) { return this.changeFormat(e ? { tag: "SPAN", attributes: { class: G, style: "font-size: " + ("number" === typeof e ? e + "px" : e) } } : null, { tag: "SPAN", attributes: { class: G } }), this.focus(); }, at.setTextColour = function (e) { return this.changeFormat(e ? { tag: "SPAN", attributes: { class: q, style: "color:" + e } } : null, { tag: "SPAN", attributes: { class: q } }), this.focus(); }, at.setHighlightColour = function (e) { return this.changeFormat(e ? { tag: "SPAN", attributes: { class: z, style: "background-color:" + e } } : e, { tag: "SPAN", attributes: { class: z } }), this.focus(); }, at.setTextAlignment = function (e) { return this.forEachBlock(function (t) { var n = t.className.split(/\s+/).filter(function (e) { return !!e && !/^align/.test(e); }).join(" "); e ? (t.className = n + " align-" + e, t.style.textAlign = e) : (t.className = n, t.style.textAlign = ""); }, !0), this.focus(); }, at.setTextDirection = function (e) { return this.forEachBlock(function (t) { e ? t.dir = e : t.removeAttribute("dir"); }, !0), this.focus(); }, at.removeAllFormatting = function (e) { if (!e && !(e = this.getSelection()) || e.collapsed) return this; for (var t = this._root, n = e.commonAncestorContainer; n && !s(n);)n = n.parentNode; if (n || (Pe(e, t), n = t), n.nodeType === F) return this; this.saveUndoState(e), Oe(e, n, n, t); for (var o, i, r = n.ownerDocument, a = e.startContainer, d = e.startOffset, l = e.endContainer, c = e.endOffset, h = r.createDocumentFragment(), u = r.createDocumentFragment(), f = b(l, c, n, t), p = b(a, d, n, t); p !== f;)o = p.nextSibling, h.appendChild(p), p = o; return I(this, h, u), u.normalize(), p = u.firstChild, o = u.lastChild, i = n.childNodes, p ? (n.insertBefore(u, f), d = ue.call(i, p), c = ue.call(i, o) + 1) : (d = ue.call(i, f), c = d), e.setStart(n, d), e.setEnd(n, c), L(n, e), xe(e), this.setSelection(e), this._updatePath(e, !0), this.focus(); }, at.increaseQuoteLevel = bt("modifyBlocks", pt), at.decreaseQuoteLevel = bt("modifyBlocks", gt), at.makeUnorderedList = bt("modifyBlocks", _t), at.makeOrderedList = bt("modifyBlocks", Nt), at.removeList = bt("modifyBlocks", Ct), R.isInline = a, R.isBlock = s, R.isContainer = d, R.getBlockWalker = l, R.getPreviousBlock = c, R.getNextBlock = h, R.areAlike = f, R.hasTagAttributes = p, R.getNearest = g, R.isOrContains = m, R.detach = N, R.replaceWith = C, R.empty = S, R.getNodeBefore = Se, R.getNodeAfter = Te, R.insertNodeInRange = ye, R.extractContentsOfRange = Ee, R.deleteContentsOfRange = be, R.insertTreeFragmentIntoRange = ke, R.isNodeContainedInRange = Le, R.moveRangeBoundariesDownTree = xe, R.moveRangeBoundariesUpTree = Oe, R.getStartBlockOfRange = Ae, R.getEndBlockOfRange = Be, R.contentWalker = Re, R.rangeDoesStartAtBlockBoundary = De, R.rangeDoesEndAtBlockBoundary = Ue, R.expandRangeToBlockBoundaries = Pe, R.onPaste = it, R.addLinks = yt, R.splitBlock = ft, R.startSelectionId = "squire-selection-start", R.endSelectionId = ht, "object" === typeof exports ? module.exports = R : "function" === typeof define && define.amd ? define(function () { return R; }) : (j.Squire = R, top !== j && "true" === e.documentElement.getAttribute("data-squireinit") && (j.editor = new R(e), j.onEditorLoad && (j.onEditorLoad(j.editor), j.onEditorLoad = null)));
    } document;
};