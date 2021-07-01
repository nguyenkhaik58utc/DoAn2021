var DownloadFileAttach = function (command, fileId) {
    var files = fileId;
    if (Array.isArray(fileId)) files = fileId.join();
    if (command == "DownloadFile") {
        Ext.net.DirectMethod.request({
            url: '/Generic/File/DownloadFile',
            isUpload: true,
            params: {
                fileId: files,
            },
        });
    }
};
var ViewFileAttach = function (command, fileId) {
    var files = fileId.join();
    if (command == "ViewFile") {
        var mask = new Ext.LoadMask(Ext.getBody(), { msg: "Chờ trong giây lát ...." });
        mask.show();
        Ext.net.DirectMethod.request({
            url: '/Generic/File/ViewFile',
            params: {
                fileId: files,
            },
            success: function () {
                mask.hide();
            },
        });
    }
    else if (command == "ViewListFile") {
        var url = '/Generic/File/ViewListFile';
        var params = { files: files };
        onDirectMethod(url, params, 'get');
    }
};
var prepareCommandViewFile = function (grid, command, record, row) {
    if (command.command == 'ViewFile' && record.data.AttachmentFileIDs.length == 1) {
        command.hidden = false;
        command.hideMode = 'display';
    }
    else if (command.command == 'ViewListFile' && record.data.AttachmentFileIDs.length > 1) {
        command.hidden = false;
        command.hideMode = 'display';
    }
};
var RenderVND = function (value, p, record) {
    return value.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,") + " VNĐ";
};
var RenderNumber = function (value, p, record) {
    return value.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
};
var AvatarRenderer = function (value) {
    var tpl = "";
    if (value == null || value == "") {
        tpl = "<img src='/Content/images/underfind.jpg' style='width:30px;height:30px;margin: -3px -6px -5.5px -6px;'/>";
    }
    else {
        tpl = "<img src='" + value + "' style='width:30px;height:30px;margin: -3px -6px -5.5px -6px;'/>";
    };
    return tpl;
};
var openEmployeeDetail = function (id) {
    var url = urlEmployeeDetail;
    var params = {
        id: id,
    };
    onDirectMethod(url, params);
};
var OpenDepartmentWindow = function (fn, multi, departmentIds) {
    var url = '/Department/Department/DepartmentWindow';
    var params = { fn: fn, multi: multi, departmentIds: departmentIds };
    onDirectMethod(url, params);
};
var onDirectMethod = function (url, params, method, handle, disabledMask) {
    method = (method == null) ? 'post' : method;
    var mask = new Ext.LoadMask(Ext.getBody(), { msg: "Đang thực hiện ...." });
    if (disabledMask == null || !disabledMask) {
        mask.show();
    };
    Ext.net.DirectMethod.request({
        method: method,
        url: url,
        params: params,
        success: function (result) {
            mask.hide();
            if (handle) handle(result);
        },
    });
};
var MessageBox = function (value) {
    var msg = "Bạn chưa lựa chọn bản ghi nào!";
    if (value == "UpdateError") {
        msg = "Có lỗi trong quá trình cập nhật dữ liệu!";
    };
    Ext.MessageBox.show({
        title: 'Thông báo', msg: msg,
        buttons: { yes: 'Đồng ý' },
        iconCls: '#Exclamation'
    });
};
function SetDisabled() {
    if (arguments != null && arguments.length > 0) {
        for (var i = 0; i < arguments.length; i++) {
            arguments[i].setDisabled(true);
        };
    };
};
function SetEnabled() {
    if (arguments != null && arguments.length > 0) {
        for (var i = 0; i < arguments.length; i++) {
            arguments[i].setDisabled(false);
        };
    };
};
var refresh = function (grid, nodeID) {
    if (nodeID == null) {
        nodeID = 'root';
    }
    var nodeRefresh = grid.getStore().getNodeById(nodeID);
    nodeRefresh.reload({ params: { node: nodeID } });
};
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
var onDelete = function (id, handle) {
    Ext.MessageBox.confirm('Thông báo', 'Bạn có muốn xóa bản ghi này ?', function (btn) {
        if (btn == 'yes') {
            var mask = new Ext.LoadMask(Ext.getBody(), { msg: "Đang thực hiện ...." });
            mask.show();
            Ext.net.DirectMethod.request({
                url: deleteUrl,
                params: {
                    id: id,
                },
                success: function (result) {
                    mask.hide();
                    if (handle) handle(result);
                },
            });
        };
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
