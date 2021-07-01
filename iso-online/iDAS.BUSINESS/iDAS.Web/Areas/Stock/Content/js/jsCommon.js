//1.show form Insert
var showFrmbyUrl = function (url) {
    Ext.net.DirectMethod.request({
        url: url
    });
}
//2.show form edit by Id
var showFrmbyUrl_Id = function (url, id) {
    Ext.net.DirectMethod.request({
        url: url,
        params: { id: id }
    });
}
//3.them mot row tren DG
function warehouseAddNewRecord(grid) {
    grid.store.insert(0, { Name: '' });
    grid.editingPlugin.startEditByPosition({ row: 0, column: 0 });
};
//4.Ham goi Action cap nhat dl cua row trn grid
var warehouseHandlecmd = function (cmd, grid, urlAdd, urlEdit, id, name, description, isuse, type) {
    if (cmd == 'save') {       
        if (name == '') {
            Ext.MessageBox.show({
                title: 'Cảnh báo', msg: 'Tên không được để trống!',
                buttons: { yes: 'Đồng ý' },
                iconCls: '#Cancel',
                fn: function (btn) {
                    return false;
                } //end fn
            }); //end msg
            return;
        } //end if
        if (id != null && id > 0 && name != '') {
            Ext.net.DirectMethod.request({
                url: urlEdit,
                params: { id: id, name: name, description: description, isuse: isuse , type:type},
                success: function (result) {
                    if (result == 'ErrorExited') {
                        Ext.MessageBox.show({
                            title: 'Lỗi', msg: 'Tên là: ' + name + ' đã tồn tại',
                            buttons: { yes: 'Đồng ý' },
                            iconCls: '#Cancel',
                            fn: function (btn) {
                                grid.getStore().reload();
                                return false;
                            }
                        }); //end msg
                    } //end if
                    if (result == 'Error') {
                        Ext.MessageBox.show({
                            title: 'Lỗi', msg: 'Cập nhật không thành công',
                            buttons: { yes: 'Đồng ý' },
                            iconCls: '#Cancel',
                            fn: function (btn) {
                                grid.getStore().reload();
                                return false;
                            }
                        }); //end msg
                    } //end if
                } //end succ
            });
        }
        else {
            Ext.net.DirectMethod.request({
                url: urlAdd,
                params: { name: name, description: description, isuse: isuse , type:type},
                success: function (result) {                  
                    if (result == 'Error') {
                        Ext.MessageBox.show({
                            title: 'Lỗi', msg: 'Cập nhật không thành công ',
                            buttons: { yes: 'Đồng ý' },
                            iconCls: '#Cancel',
                            fn: function (btn) {
                                grid.getStore().reload();
                                return false;
                            }
                        }); //end msg
                    }
                }
                //end success
            });
        }
    }
    //command reject
    if (cmd == 'reject') {
        grid.getStore().reload();
    }
};
var warehouseDeleteRecord = function (url, records) {
    var label = '';  
    if (records.length > 1) { label = ' các '; } else { label = ' '; }
    Ext.MessageBox.show({
        title: 'Xác nhận', msg: 'Bạn có chắc chắn muốn xóa' + label + 'Bản ghi đã chọn không?',
        buttons: { yes: 'Đồng ý', no: 'Không' },
        iconCls: '#Information',
        fn: function (btn) {
            if (btn == 'yes') {
                var aId = new Array();
                var strId = '';
                for (var i = 0, r; r = records[i]; i++) {
                    aId[i] = records[i].get('ID');
                } //end for
                strId = aId.join();
                Ext.net.DirectMethod.request({
                    url: url,
                    params: { stringId: strId }
                });
            } //end if
        } //end fn
    }); //end msg  
};
//tìm kiếm dữ liệu
//5. Tim kiem theo Name
var warehouseSearchFieldTriggerClick = function (field, trigger, index) {
    var me = field,
                store = me.up("grid").getStore(),
                value = me.getValue();
    if (value.length > 0) {

        if (index == 0) {
            me.setValue('');
            store.clearFilter();
            trigger.hide();
        }
        else {
            if (value.length > 0) {
                store.filterBy(warehouseGetRecordFilter(value));
                me.getTrigger(0).show();
            }
        }
    }
};
var warehouseFilterString = function (value, dataIndex, record) {
    var val = record.get(dataIndex);

    if (typeof val != "string") {
        return value.length == 0;
    }
    return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
};
var warehouseGetRecordFilter = function (txtsearch) {
    var f = [];
    f.push({
        filter: function (record) {
            return warehouseFilterString(txtsearch || "", "Name", record);
        }
    });
    var len = f.length;
    return function (record) {
        for (var i = 0; i < len; i++) {
            if (!f[i].filter(record)) {
                return false;
            }
        }
        return true;
    };
};
//end Tìm kiếm
//1.cbo lua chon so row/ 1 trang
var warehouseOnComboBoxSelect = function (combo) {
    var store = combo.up("gridpanel").getStore();
    store.pageSize = parseInt(combo.getValue(), 10);
    store.load();
};

