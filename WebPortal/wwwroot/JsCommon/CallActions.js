function CallActions(Method, data, success, error, async, Types)
{
    $.ajax({

        url: Method,
        type: Types,
        headers:
        {
            "Accept-Language": sessionStorage.getItem("LangSelected"),
        },
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        async: async,
        success: success,
        error: error,
    });
}
function callServiceBackendMultiPart(Method, data, success, error, async, Types, ContentType, ProcessData)
{
    $.ajax({

        url: Method,
        type: Types,
        //crossDomain: true,
        //processlData: false,
        headers:
        {
            "Accept-Language": sessionStorage.getItem("LangSelected"),
        },

        data: data,
        contentType: ContentType,
        processData: ProcessData,
        //datatype: "json",
        async: async,
        success: success,
        error: error,
    });
}

