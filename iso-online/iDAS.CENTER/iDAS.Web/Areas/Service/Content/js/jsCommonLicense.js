//1.cbo lua chon so row/ 1 trang
var onComboBoxSelect = function (combo) {
    var store = combo.up("gridpanel").getStore();
    store.pageSize = parseInt(combo.getValue(), 10);
    store.load();
};
//2
// Hàm hien thi context menu
var itemcontextmenu = function (grid, record, item, index, e, eOpts) {
    e.stopEvent();
    App.cmMenu.showAt(e.getXY());
};
//4
// Hàm lấy tên ảnh của imageButton
var getImageName = function () {
    var imgUrl = document.getElementById("IconImage").getAttribute('src');
    return imgUrl;
};

