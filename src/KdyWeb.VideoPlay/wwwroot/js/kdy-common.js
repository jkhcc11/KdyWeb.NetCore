var kdyCommon = {
    /**
     * 发送Post|Put|Patch 请求
     * @param {any} method 请求方法
     * @param {any} url 请求地址
     * @param {any} pd  请求数据
     * @param {any} successCallBack  成功回调
     * @param {any} contentType   application/json | application/x-www-form-urlencoded请求类型
     */
    kdySendHttp: function (method, url, pd, successCallBack, contentType) {
        method = method || 'POST';
        contentType = contentType || 'application/json';
        $.ajax({
            url: url,
            data: JSON.stringify(pd),
            type: method,
            headers: {
                'Authorization': reqToken
            },
            contentType: contentType,
            success: function (json) {
                successCallBack(json);
            },
            error: function (request, status, thrown) {
                console.log(request);
                // console.log(status);
                //  console.log(thrown);
            }
        });
    }
}