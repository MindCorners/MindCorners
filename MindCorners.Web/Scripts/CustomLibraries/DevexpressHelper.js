
var command = "";
var DevexpressHelper = {
    DropDownEdit: (function () {
        return {
            OnListBoxSelectedIndexChanged: function (listbox, args, dropdown) {
                var selectedItems = listbox.GetSelectedItems();
                dropdown.SetText(this.GetSelectedItemsText(selectedItems));
            },
            GetSelectedItemsText: function (items) {
                var texts = [];
                for (var i = 0; i < items.length; i++)
                    texts.push(items[i].text);
                return texts.join(textSeparator);
            },
            OnDropDownTextChanged: function (dropDown, args, listbox) {
                listbox.UnselectAll();
                var texts = dropDown.GetText().split(textSeparator);
                var values = GetValuesByTexts(texts, listbox);
                listbox.SelectValues(values);
                UpdateText(listbox, dropDown); // for remove non-existing texts
            },
            UpdateText: function (listbox, dropdown) {
                var selectedItems = listbox.GetSelectedItems();
                dropdown.SetText(GetSelectedItemsText(selectedItems));
            }
        };
    } ()),
    Grid: function (container, template, btnEdit, canEdit) {
        var _methods = {
            // Reload GridOperationMenu
            RefreshGridOperationMenu: function (id) {
                container = container.jquery ? container : jQuery(container);
                template = template.jquery ? template : jQuery(template);
                container.html(template.tmpl({ id: id }));
            },

            // Temporary unused
            OnGridInit: function (s) {
               
            },

            OnBeginCallback: function (s, e) {
                command = e.command;
            },
            // Temporary unused
            OnGridEndCallback: function (s) {
                if (command == "APPLYCOLUMNFILTER") {
                    s.SetFocusedRowIndex(-1);
                    s.SetFocusedRowIndex(0);
                    //_methods.RefreshGridOperationMenu(id);
                }
                //if(parseInt(s.cpGridRowCount) == 0)
                //_methods.RefreshGridOperationMenu();OnBeginCallback

            },

            // Reload GridOperationMenuPartial on focused row changed
            OnGridFocusedRowChanged: function (s) {
                var id = s.GetRowKey(s.GetFocusedRowIndex());
                _methods.RefreshGridOperationMenu(id);
            },

            // Edit on double click
            OnGridDoubleClick: function (s) {
                btnEdit = btnEdit.jquery ? btnEdit : jQuery(btnEdit);
                if (btnEdit.length) {
                    var id = s.GetRowKey(s.GetFocusedRowIndex());
                    _methods.RefreshGridOperationMenu(id);
                    window.location.href = btnEdit.attr('href');
                }
            },

            // If only one column left in grid, don't allow to drag it into customization window
            OnGridColumnStartDragging: function (s, e) {
                var columnCount = s.cpGridColumnCount;
                if (columnCount == 1 && !e.column.inCustWindow)
                    e.column.allowDrag = false;
            }
        };

        return _methods;
    }
};