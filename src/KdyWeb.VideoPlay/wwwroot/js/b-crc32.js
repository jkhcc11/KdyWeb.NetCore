//参考：https://github.com/esterTion/BiliBili_crc2mid
; function BCrc32() {
    this.init();
}
BCrc32.prototype = {
    init: function () {
        this.CrcPOLYNOMIAL = 0xEDB88320;
        this.crcTable = [];
        this.create_table();
    },
    /**
     * 创建crc表
     */
    create_table: function () {
        var crc,
            i,
            j,
            that = this;
        if (that.crcTable.length > 0) {
            return;
        }

        for (i = 0; i < 256; ++i) {
            crc = i;
            for (j = 0; j < 8; ++j) {
                if ((crc & 1) != 0) {
                    crc = that.CrcPOLYNOMIAL ^ (crc >>> 1);
                }
                else {
                    crc >>>= 1;
                }
            }
            that.crcTable[i] = crc;
        }
    },
    /**
     * crc计算
     * @param {any} input 待处理参数 
     * @returns 
     */
    crc32: function (input) {
        if (typeof (input) != 'string') {
            input = input.toString();
        }

        var crcStart = 0xFFFFFFFF,
            len = input.length,
            index,
            that = this;
        for (var i = 0; i < len; ++i) {
            index = (crcStart ^ input.charCodeAt(i)) & 0xff;
            crcStart = (crcStart >>> 8) ^ that.crcTable[index];
        }

        return crcStart;
    },
    /**
     * crc计算获取index
     * @param {*} input  待处理参数
     * @returns 
     */
    crc32lastindex: function (input) {
        if (typeof (input) != 'string') {
            input = input.toString();
        }

        var crcStart = 0xFFFFFFFF,
            len = input.length,
            index,
            that = this;
        for (var i = 0; i < len; ++i) {
            index = (crcStart ^ input.charCodeAt(i)) & 0xff;
            crcStart = (crcStart >>> 8) ^ that.crcTable[index];
        }

        return index;
    },
    /**
     * 获取某个值在表中的Index
     * @param {any} findTemp 待查找的值
     * @returns 
     */
    getCrcIndex: function (findTemp) {
        var that = this;
        for (var i = 0; i < 256; i++) {
            if (that.crcTable[i] >>> 24 == findTemp)
                return i;
        }
        return -1;
    },
    deepCheck: function (i, index) {
        var tc = 0x00,
            str = '',
            that = this,
            hash = that.crc32(i);
        tc = hash & 0xff ^ index[2];
        if (!(tc <= 57 && tc >= 48))
            return [0];
        str += tc - 48;
        hash = that.crcTable[index[2]] ^ (hash >>> 8);
        tc = hash & 0xff ^ index[1];
        if (!(tc <= 57 && tc >= 48))
            return [0];
        str += tc - 48;
        hash = that.crcTable[index[1]] ^ (hash >>> 8);
        tc = hash & 0xff ^ index[0];
        if (!(tc <= 57 && tc >= 48))
            return [0];
        str += tc - 48;
        hash = that.crcTable[index[0]] ^ (hash >>> 8);
        return [1, str];
    },
    /**
     * crc值反查
     * @param {any} crcValue crc值
     */
    antiCheck: function (crcValue) {
        var ht = parseInt('0x' + crcValue) ^ 0xffffffff,
            that = this,
            sNum,
            i,
            lastindex,
            deepCheckData,
            index = new Array(4),
            minIndex = 0,
            maxIndex = 999999;
        for (i = 3; i >= 0; i--) {
            index[3 - i] = that.getCrcIndex(ht >>> (i * 8));
            sNum = that.crcTable[index[3 - i]];
            ht ^= sNum >>> ((3 - i) * 8);
        }

        for (i = minIndex; i < maxIndex; i++) {
            lastindex = that.crc32lastindex(i);
            if (lastindex == index[3]) {
                deepCheckData = that.deepCheck(i, index)
                if (deepCheckData[0])
                    break;
            }
        }
        if (i == maxIndex)
            return -1;
        return i + '' + deepCheckData[1];
    }
}