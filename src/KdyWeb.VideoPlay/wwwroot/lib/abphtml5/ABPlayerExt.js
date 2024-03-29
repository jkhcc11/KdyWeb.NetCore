﻿! function () {
    var i, t, n, o, e, c, s, a, h, r = function (i) {
        return i - .5 * Math.PI
    },
        l = function (i, t, n, o) {
            return [i + Math.cos(o) * n, t + Math.sin(o) * n]
        },
        f = function (i, t, n) {
            var i = parseInt(i, 16),
                t = parseInt(t, 16),
                o = i >>> 16,
                e = i >> 8 & 255,
                c = 255 & i,
                s = t >>> 16,
                a = t >> 8 & 255,
                h = 255 & t,
                r = o + (s - o) * n | 0,
                l = e + (a - e) * n | 0,
                f = c + (h - c) * n | 0,
                g = 65536 * r + 256 * l + f;
            for (g = g.toString(16); g.length < 6;) g = "0" + g;
            return g
        },
        g = Math.sin,
        d = Math.PI;
    window.dots = {
        running: !1,
        ticking: !1,
        config: {
            color: ["FFFFFF", "FFFFFF"],
            r: 20,
            thick: 2,
            spinningSpeed: 700
        },
        currentColor: "",
        canvas: null,
        context: null,
        init: function (i) {
            for (var t in i) this.config[t] = i[t]
        },
        mainLoop: function () {
            if (this.context.clearRect(0, 0, this.canvas.width, this.canvas.height), this.running) {
                var u, v, m = Date.now(),
                    w = (m - h) % 1400,
                    p = window.devicePixelRatio,
                    F = this.canvas.width / 2,
                    x = this.canvas.height / 2,
                    k = (m - h) / 1400 | 0,
                    C = this.context;
                k != a && (c = s, s = (c + 1400 / n - .1 + 5 / 3) % 2, a = k, o = this.config.color[this.config.color.indexOf(o) + 1], e = this.config.color[this.config.color.indexOf(o) + 1], this.currentColor = o), C.fillStyle = "#" + o, u = c + w / n, 700 > w ? v = u + .1 + (5 / 3 - .1) * (1 * g((w - 350) / 700 * d) + 1) / 2 : (v = u + 5 / 3, u = v - .1 - (5 / 3 - .1) * (1 - (1 * g((w - 1050) / 700 * d) + 1) / 2)), w > 1200 && (C.fillStyle = "#" + f(o, e, (w - 1200) / 200)), u = r(u * d), v = r(v * d);
                var P = l(F, x, i * p, u),
                    y = l(F, x, (i + t) * p, v);
                C.beginPath(), C.moveTo(P[0], P[1]), C.arc(F, x, i * p, u, v), C.lineTo(y[0], y[1]), C.arc(F, x, (i + t) * p, v, u, !0), C.lineTo(P[0], P[1]), C.fill(), this.ticking || (requestAnimationFrame(this.mainLoopCaller), this.ticking = !0)
            }
        },
        mainLoopCaller: function () {
            dots.ticking = !1, dots.mainLoop()
        },
        runTimer: function () {
            var e = document.getElementById(dots.config.id);
            if (null != e && !this.running) {
                var c = e.querySelector("canvas");
                null == c && (c = document.createElement("canvas"), c.style.width = this.config.width || "", c.style.height = this.config.height || "", e.appendChild(c), this.canvas = c, c.width = c.offsetWidth * devicePixelRatio, c.height = c.offsetHeight * devicePixelRatio, this.context = c.getContext("2d")), this.running = !0, h = Date.now(), a = -1, s = 0, o = this.config.color[this.config.color.length - 2], t = this.config.thick, i = this.config.r, n = this.config.spinningSpeed, this.ticking || (requestAnimationFrame(this.mainLoopCaller), this.ticking = !0)
            }
        },
        stopTimer: function () {
            this.running = !1
        }
    }, window.addEventListener("resize", function () {
        if (null != dots.canvas) {
            var i = window.devicePixelRatio;
            dots.canvas.width = dots.canvas.offsetWidth * i, dots.canvas.height = dots.canvas.offsetHeight * i
        }
    })
}(); /*end*/