var iConRenderer = function (value) {
    var tpl = "<img src='" + value + "' />";
    return tpl;
};
var onComboBoxSelect = function (combo, e) {
    var store = combo.up("gridpanel").getStore();
    store.pageSize = parseInt(combo.getValue(), 10);
    store.reload({ params: { page: 1, limit: store.pageSize } });
};
var onRestore = function () {
    Ext.MessageBox.confirm('Thông báo', 'Bạn có muốn khôi phục dữ liệu ?', function (btn) {
        if (btn == 'yes')
            Ext.net.DirectMethod.request({
                url: restoreUrl,
                params: {
                }
            });
    })
};
var onOpen = function () {
    App.WindowAdd.show();
}
var onClosed = function () {
    App.WindowAdd.hide();
    App.WindowAdd.down('form').reset();
}
var onAdd = function () {
    this.up('window').down('form').submit({
        url: insertUrl,
        success: function (form, action) {
            if (action.result.success) onClosed();
        }
    });
}
var onDelete = function (id) {
    Ext.MessageBox.confirm('Thông báo', 'Bạn có muốn xóa bản ghi này ?', function (btn) {
        if (btn == 'yes')
            Ext.net.DirectMethod.request({
                url: deleteUrl,
                params: {
                    id: id,
                }
            });
    })
};
var onEdit = function (editor, e) {
    if (!(e.value === e.originalValue || (Ext.isDate(e.value) && Ext.Date.isEqual(e.value, e.originalValue)))) {
        Ext.net.DirectMethod.request({
            url: updateUrl,
            params: {
                data: e.record.data,
            }
        });
    }
};
var onRequest = function (id, url, titleConfirm, messageConfirm) {
    Ext.MessageBox.confirm(titleConfirm, messageConfirm, function (btn) {
        if (btn == 'yes') {
            var mask = new Ext.LoadMask(Ext.getBody(), { msg: "Đang thực hiện ...." });
            mask.show();
            Ext.net.DirectMethod.request({
                url: url,
                params: {
                    id: id,
                },
                success: function (result) {
                    //Ext.MessageBox.hide();
                    mask.hide();
                },
            });
        }
    })
};

/*handle post request to server---------------------------------------------*/
var postRequest = function (url, data, titleConfirm, messageConfirm) {
    Ext.MessageBox.confirm(titleConfirm, messageConfirm, function (btn) {
        if (btn == 'yes') {
            var mask = new Ext.LoadMask(Ext.getBody(), { msg: "Đang thực hiện ...." });
            mask.show();
            Ext.net.DirectMethod.request({
                url: url,
                params: {
                    data: data,
                },
                success: function (result) {
                    mask.hide();
                },
            });
        }
    })
};
/*--------------------------------------------------------------------------*/


var onOpenWindow = function (window) {
    window.show();
}
var onClosedWindow = function (window) {
    var form = window.down('form');
    if (form) form.reset();
    window.close();
}
var onSubmitWindow = function (window) {
    window.down('form').submit({
        success: function (form, action) {
            if (action.result.success) {
                onClosedWindow(window);
            }
        }
    });
}

//handle delete all records on grid-----------------------------------------*/
var deleteAll = function (grid, url) {
    var ids = getSelectedRowIds(grid);
    if (ids.length <= 0) return;
    postRequest(url, ids, "Thông báo", "Bạn có muốn xóa toàn bộ bản ghi đã chọn ?");
};
/*--------------------------------------------------------------------------*/


//get all records are selected----------------------------------------------*/
var getSelectedRowIds = function (grid) {
    var source = grid.getRowsValues({ selectedOnly: true });
    var ids = '';
    Ext.each(source, function (record) {
        ids = ids + ',' + record.ID;
    });
    return ids;
};
/*--------------------------------------------------------------------------*/