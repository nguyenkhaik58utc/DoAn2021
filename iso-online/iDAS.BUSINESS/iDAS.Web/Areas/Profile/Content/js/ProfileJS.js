//4.Ham goi Action cap nhat dl cua row trn grid
var profileHandlecmd = function (cmd, grid, urlAdd, urlEdit, records) {
    if (cmd == 'save') {
        // alert(type);;
        var id = records[0].get('ID'), name = records[0].get('Name'), description = records[0].get('Note'), isuse = records[0].get('IsActive');
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
            //sua du lieu          
            Ext.net.DirectMethod.request({
                url: urlEdit,
                params: { id: id, name: name, description: description, isuse: isuse },
                timeout: 50000,
                eventMask: { showMask: true, Msg: "Please wait" },
                success: function (result) {
                    if (result == 'ErrorExited') {
                        Ext.MessageBox.show({
                            title: 'Lỗi', msg: 'Tên là: ' + name + ' đã tồn tại',
                            buttons: { yes: 'Đồng ý' },
                            iconCls: '#Cancel',
                            fn: function (btn) {
                                records[0].reject();
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
                                records[0].reject();
                                return false;
                            }
                        }); //end msg
                    } //end if
                } //end succ
            });
        }
        else {

            //them du lieu
            Ext.net.DirectMethod.request({
                url: urlAdd,
                params: { name: name, description: description, isuse: isuse },
                timeout: 50000,
                eventMask: { showMask: true, Msg: "Please wait" },
                success: function (result) {
                    if (result == 'ErrorExited') {
                        Ext.MessageBox.show({
                            title: 'Lỗi', msg: 'Tên là: ' + name + ' đã tồn tại',
                            buttons: { yes: 'Đồng ý' },
                            iconCls: '#Cancel',
                            fn: function (btn) {
                                records[0].reject();
                                return false;
                            }
                        }); //end 
                    }
                    if (result == 'Error') {
                        Ext.MessageBox.show({
                            title: 'Lỗi', msg: 'Thêm mới không thành công ',
                            buttons: { yess: 'Đồng ý' },
                            iconCls: '#Cancel',
                            fn: function (btn) {
                                records[0].reject();
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
        records[0].reject();
    }
};
var profileDeleteRecord = function (url, records, type) {
    var label = '';
    if (records.length > 1) { label = ' các '; } else { label = ' '; }
    Ext.MessageBox.show({
        title: 'Xác nhận', msg: 'Bạn có chắc chắn muốn xóa' + label + 'bản ghi đã chọn không?',
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
                    params: { id: strId, table: type },
                    timeout: 50000,
                    eventMask: { showMask: true, Msg: "Please wait" },
                    success: function (result) {
                        //TH Xoa bi loi -> thong bao
                        if (result == 'Error') {
                            Ext.MessageBox.show({
                                title: 'Lỗi', msg: 'Xóa không thành công ',
                                buttons: { yes: 'Đồng ý' },
                                iconCls: '#Cancel',
                                fn: function (btn) {
                                    return false;
                                }
                            }); //end msg
                        }
                    }
                }); //end

            } //end if
        } //end fn
    }); //end msg  
};
var onComboBoxSelect = function (combo) {
    var store = combo.up("gridpanel").getStore();
    store.pageSize = parseInt(combo.getValue());
    store.load();
};