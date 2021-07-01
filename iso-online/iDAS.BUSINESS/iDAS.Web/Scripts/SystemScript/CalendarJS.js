var CompanyX = {
    getCalendar: function () {
        return CompanyX.CalendarPanel1;
    },

    getStore: function () {
        return CompanyX.EventStore1;
    },

    getWindow: function () {      
        return CompanyX.EventWindow1;
    },

    viewChange: function (p, vw, dateInfo) {
        var win = this.getWindow();
        if (win) {
            win.hide();
        }

        if (dateInfo) {
            // will be null when switching to the event edit form, so ignore
            this.DatePicker1.setValue(dateInfo.activeDate);
            this.updateTitle(dateInfo.viewStart, dateInfo.viewEnd);
        }
    },
    updateTitle: function (startDt, endDt) {
        var msg = '',
            fmt = Ext.Date.format;
        if (Ext.Date.clearTime(startDt).getTime() == Ext.Date.clearTime(endDt).getTime()) {
            msg = fmt(startDt, 'F j, Y');
        } else if (startDt.getFullYear() == endDt.getFullYear()) {
            if (startDt.getMonth() == endDt.getMonth()) {
                msg = fmt(startDt, 'F j') + ' - ' + fmt(endDt, 'j, Y');
            } else {
                msg = fmt(startDt, 'F j') + ' - ' + fmt(endDt, 'F j, Y');
            }
        } else {
            msg = fmt(startDt, 'F j, Y') + ' - ' + fmt(endDt, 'F j, Y');
        }
        this.Panel1.setTitle(msg);
    },

    setStartDate: function (picker, date) {
        this.getCalendar().setStartDate(date);
    },

    rangeSelect: function (cal, dates, callback) {
        this.record.show(cal, dates);
        this.getWindow().on('hide', callback, cal, { single: true });
    },

    dayClick: function (cal, dt, allDay, el) {
        this.record.show.call(this, cal, {
            StartDate: dt,
            IsAllDay: allDay
        }, el);
    },

    record: {
        addFromEventDetailsForm: function (win, rec) {
            CompanyX.ShowMsg('Lịch ' + rec.data.Title + ' vừa được thêm mới');
        },

        add: function (win, rec) {
            win.hide();
            CompanyX.getStore().add(rec);         
            addCalendar(rec.data.Title, Ext.Date.format(rec.data.StartDate, 'd/m/Y g:i A'), Ext.Date.format(rec.data.EndDate, 'd/m/Y g:i A'), rec.data.CalendarId);                     
        },

        updateFromEventDetailsForm: function (win, rec) {
            CompanyX.ShowMsg('Lịch ' + rec.data.Title + ' vừa được sửa');
        },

        update: function (win, rec) {          
            win.hide();
            rec.commit();
            updateCalendar(rec.data.EventId, rec.data.Title, Ext.Date.format(rec.data.StartDate, 'd/m/Y g:i A'), Ext.Date.format(rec.data.EndDate, 'd/m/Y g:i A'), rec.data.CalendarId);
        },

        removeFromEventDetailsForm: function (win, rec) {
            CompanyX.ShowMsg('Lịch ' + rec.data.Title + ' đã bị xóa');
        },

        remove: function (win, rec) {
            CompanyX.getStore().remove(rec);
            deleteCalendar(rec.data.EventId);
            win.hide();           
        },

        edit: function (win, rec) {
            win.hide();
            rec.commit();
            CompanyX.getCalendar().showEditForm(rec);
        },

        resize: function (cal, rec) {
            rec.commit();
            updateCalendar(rec.data.EventId, rec.data.Title, Ext.Date.format(rec.data.StartDate, 'd/m/Y g:i A'), Ext.Date.format(rec.data.EndDate, 'd/m/Y g:i A'), rec.data.CalendarId);
        },

        move: function (cal, rec) {
            rec.commit();
            updateCalendar(rec.data.EventId, rec.data.Title, Ext.Date.format(rec.data.StartDate, 'd/m/Y g:i A'), Ext.Date.format(rec.data.EndDate, 'd/m/Y g:i A'), rec.data.CalendarId);
        },

        show: function (cal, rec, el) {
            CompanyX.getWindow().show(rec, el);
        },

        saveAll: function () {
            CompanyX.getStore().submitData({
                mappings: false
            }, CompanyX.getStore().submitDataUrl);
        }
    }
};