function SetValueCt(ct1, value1) {
    $('#' + ct1).html(value1);    
}
function PostAjaxSetValueCt(url, id, ct1) {
    $.ajax(
        {
            url: url,
            type: 'get',
            data: { id: id},
            success: function (rs) {               
                SetValueCt(ct1, rs);
            }
        }
     )
}