/**
 * bootstrap-treetable
 * v1.0.1
 * @author leo.du
 * @url https://gitee.com/tojsp/bootstrap-treetable
 * @description 
 * 此文件是基于作者：swifly， 地址: https://gitee.com/cyf783/bootstrap-treetable/ 改造而来
 * 
 * 移除的功能如下：
 * 1. 去掉了固定表头，固定列
 * 2. 去掉了窗口缩放时自适应,只使用bootstrap的自适应
 * 
 * 增加的功能如下:
 * #. 使用ES6 模板字符串(Template String)增强版的字符串, 节省大量的逻辑判断和字符拼接，有效的减少了代码量
 * #. 增加了自定义右侧工具栏，可以指定位置，不限制于一定要指定toolbar，此功能使用rightToolbar实现
 * #. 增加了checkbox的级联选中功能hierarchical，在子结点或者父结点选中时，会自动根据父子关系选中对应的checkbox
 * #. 增加工具栏展开所有，折叠所有两个按钮，showExpandCollapse
 * #. 增加了加载提示loadingTips, 可以配置成html代码或者是文字，也可以做成动画
 * #. 增加onLoadComplte, onLoading这两个事件监听，方便在此自定义事件
 * #. column列增加了cssClass字段，方便用css样式来控制td的宽度和美化，而不是之前的只有指定固定width
 * #. 工具栏增加了批量排序按钮showBatchSort，方便对列表进行批量排序，但是这个功能需要自己定义代码实现(因为业务字段不好统一)，界面上只是提供了一个按钮
 * #. 增加了toolTip提示，美化了title
 */
(function($) {
    "use strict";

    var pluginName = 'bootstrap-tree-table';

    var BootstrapTreeTable = function(el, options) {
        this.options = options;
        this.$el = $(el);
        this.$el_ = this.$el.clone();
        this.$headerBox = null;//头部盒子
        this.$leftBox = null;//左侧固定列盒子
        this.data_list = null; //用于缓存格式化后的数据-按父分组
        this.data_obj = null; //用于缓存格式化后的数据-按id存对象
        this.hiddenColumns = []; //用于存放被隐藏列的field
        this.lastAjaxParams = null; //用户最后一次请求的参数
        this.hasSelectItem = false; // 是否有radio或checkbox
        this.leftFixedColumns = [];//左侧固定列集合
        this.noFixedColumns = [];//非固定列集合
        this.expandColumnIsFixed = false;//展开列是否是固定列
        this.selectedDataIds = [];//已选记录集合
        this.expandColumnField = null//展开列的字段名
        this.init();//初始化
    };
    // 初始化
    BootstrapTreeTable.prototype.init = function(parms) {
        // 初始化配置
        this.initOptions();
        // 初始化容器
        this.initContainer();
        // 初始化工具栏
        this.initToolbar();
        // 初始化表头
        this.initHeader();
        // 初始化表体
        this.initBody();
        // 初始化数据服务
        this.initServer(parms);
    };
    // 初始化配置
    BootstrapTreeTable.prototype.initOptions = function() {
        var self = this;
        $.each(self.options.columns, function(i, column) {
            column = $.extend({}, BootstrapTreeTable.COLUMN_DEFAULTS, column);
            if(column.width){
                column.width += (column.width.indexOf("%") == -1&&column.width.indexOf("px") == -1)?"px":"";
            }
            if (self._isCheckRadio(column)) {
                // 判断有没有选择列
                self.hasSelectItem = true;
                // 选择列永远在左边第一列
                self.leftFixedColumns.push(column);
                
                if (self.options.hierarchical && !column.checkbox) { // 给出警告提示
                    console.warn('options hierarchical = true,  must match column checkbox');
                }
            } else {
                if(column.fixed){
                    if(column.fixed=="left" || column.fixed == true){
                        self.leftFixedColumns.push(column);
                    }
                    if(self.options.expandColumn == i){
                        self.expandColumnIsFixed=true;
                    }
                } else {
                    if (!(typeof column.visible == "undefined" || column.visible == true)) {
                        self.hiddenColumns.push(column.field);
                    }
                    self.noFixedColumns.push(column);
                }
            }
            if(self.options.expandColumn == i){
                self.expandColumnField=column.field;
            }
            //self.options.columns[i]=column;
        });
        // 如果没有固定列就放回去吧。。。
        if(self.leftFixedColumns.length==1){
            self.noFixedColumns.unshift(self.leftFixedColumns[0])
            self.leftFixedColumns.pop();
        }
    };
    
    BootstrapTreeTable.prototype._isCheckRadio = function(column) {
        return column.checkbox || column.radio;
    }
    // 初始化容器
    BootstrapTreeTable.prototype.initContainer = function() {
        var self = this;
        // 在外层包装一下div，样式用的bootstrap-table的
        var $container = $(`<div class='${pluginName}'></div>`);
        var $treetable = $("<div class='treetable-box'></div>");
        var $bodyBox = $("<div class='treetable-body-box'></div>");
        self.$el.before($container);
        $container.append($treetable);
        $treetable.append($bodyBox);
        $bodyBox.append(self.$el);
        self.$el.addClass("table treetable-table");
        if (self.options.striped) {
            self.$el.addClass('table-striped');
        }
        if (self.options.bordered) {
            $treetable.addClass('treetable-bordered');
        }
        if (self.options.condensed) {
            self.$el.addClass('table-condensed');
        }
        if (self.options.width) {
            $container.css('width',self.options.width);
            $treetable.css('width',self.options.width);
        }
        // 默认高度
        if (self.options.height) {
            $bodyBox.css("height", self.options.height);
        }
        self.$el.html("");
    };
    // 初始化工具栏
    BootstrapTreeTable.prototype.initToolbar = function() {
        var self = this;
        
        var $rightToolbar = $('<div class="btn-group tool-right">');
        if (self.options.rightToolbar) { // 如果定义了rightToobar，则忽略上面的工具栏，因为一般的工具栏都是自己的代码实现的，不好通用
            $(self.options.rightToolbar).append($rightToolbar).addClass('box-tools');
        } else {
            var $toolbar = $("<div class='treetable-bars'></div>");
            if (self.options.toolbar) {
                $(self.options.toolbar).addClass('tool-left');
                $toolbar.append($(self.options.toolbar));
            }
            $toolbar.append($rightToolbar);
            self._getContainer().prepend($toolbar);
        }
        // 是否显示批量排序, 默认是不显示
        const _btnStyle = self.options.toolbarBtnStyle;
        if (self.options.showBatchSort) {
            $rightToolbar.append(self._initToolbarButton(_btnStyle, 'batch_sort_btn', 'Sorting', '批量排序', self.options.toolBatchSortClass));
        }
        // 是否显示全选按钮
        // 是否显示取消全选按钮
        if (self.options.showCheckAll) {
            $rightToolbar.append(self._initToolbarButton(_btnStyle, 'check_all_btn', 'Check All', '全选', self.options.toolCheckAllClass));
            $rightToolbar.append(self._initToolbarButton(_btnStyle, 'uncheck_all_btn', 'Uncheck All', '取消全选', self.options.toolUncheckAllClass));
        }
        // 是否显示展开折叠所有按钮
        if (self.options.showExpandCollapse) {
            $rightToolbar.append(self._initToolbarButton(_btnStyle, 'expand_btn', 'Expand', '展开所有', self.options.toolExpandClass));
            $rightToolbar.append(self._initToolbarButton(_btnStyle, 'collapse_btn', 'Collapse', '折叠所有', self.options.toolCollapseClass));
        }
        // 是否显示刷新按钮
        if (self.options.showRefresh) {
            $rightToolbar.append(self._initToolbarButton(_btnStyle, 'refresh_btn', 'Refresh', '刷新', self.options.toolRefreshClass));
        }
        // 是否显示隐藏、显示列选项
        if (self.options.showColumns) {
            var $columns_div = $(`<div class="btn-group pull-right" data-toggle="tooltip" title="隐藏/展示列">
                                <button type="button" aria-label="columns" class="btn ${self.options.toolbarBtnStyle} dropdown-toggle"
                                 data-toggle="dropdown" aria-expanded="false">
                                 <i class="${self.options.toolColumnsClass}"></i> <span class="caret"></span></button></div>`);
            var $columns_ul = $('<ul class="dropdown-menu dropdown-menu-right columns" role="menu"></ul>');
            // 固定列不能隐藏
            self._initShowColumns($columns_ul, self.leftFixedColumns, true);
            self._initShowColumns($columns_ul, self.noFixedColumns, false);
            $rightToolbar.append($columns_div.append($columns_ul));
        }
    };
    
    // 定位到容器, 即顶层包裹的div
    BootstrapTreeTable.prototype._getContainer = function() {
        return this.$el.closest('.' + pluginName);
    };
    
    // 统一方式创建工具栏的按钮, 使用模板统一输出
    BootstrapTreeTable.prototype._initToolbarButton = function(btnStyle, btnClass, ariaLabel, title, iconCls) {
        return $(`<button class="btn ${btnStyle} ${btnClass}" type="button" aria-label="${ariaLabel}"
             data-toggle="tooltip" title="${title}"><i class="${iconCls}"></i></button>`);
    };
    
    // 输出隐藏显示列
    BootstrapTreeTable.prototype._initShowColumns = function($dom, columns, isDisabled) {
        var self = this;
        $.each(columns, function(i, column) {
            if (!self._isCheckRadio(column)) {
                var _li = $(`<li role="menuitem"><label><input type="checkbox" checked="checked" ${isDisabled?"disabled":""} 
                    data-field="${column.field}" value="${column.field}"> ${column.title}</label></li>`);
                $dom.append(_li);
            }
        });
    };
    // 初始化隐藏列
    BootstrapTreeTable.prototype.initHiddenColumns = function() {
        var self = this;
        $.each(self.hiddenColumns, function(i, field) {
            // 固定列不能隐藏
            var _index = $.inArray(field, self.leftFixedColumns);
            if (!(_index > -1)) {
                self._getContainer().find("." + field + "_cls").hide();
                let _rb = self.options.rightToolbar, $dom;
                if (_rb) {
                    $dom = $(_rb).find('.columns label');
                } else {
                    $dom = $(".bootstrap-tree-table .treetable-bars .columns label");
                }
                $dom.find("input[value='" + field + "']").prop("checked", false);
            }
        });
    };
    // 初始化表头
    BootstrapTreeTable.prototype.initHeader = function() {
        var self = this;
        var $thr = $('<tr></tr>');
        // 因有可能出现冻结列，所以这里合并列配置
        var _columns = self.leftFixedColumns.concat(self.noFixedColumns);
        $.each(_columns, function(i, column) {
            var $th;
            // 判断是不是选择列
            if (i == 0 && self._isCheckRadio(column)) {
                let width = column.width?column.width:'30px';
                $th = $('<th style="width:'+ width +'"></th>');
            } else {
                let _cssClass = column.field + '_cls';
                _cssClass = column.cssClass ? column.cssClass + ' ' + _cssClass : column.field + '_cls';
                $th = $('<th style="' + ((column.width) ? ('width:' + column.width) : '') + '" class="' + _cssClass + '"></th>');
            }
            $th.text(column.title);
            $thr.append($th);
        });
        self.$el.append($('<thead class="treetable-thead"></thead>').append($thr));
    };
    // 初始化表体
    BootstrapTreeTable.prototype.initBody = function() {
        this.$el.append($('<tbody class="treetable-tbody"></tbody>'));
    };
    // 初始化数据服务
    BootstrapTreeTable.prototype.initServer = function(parms) {
        var self = this;
        // 加载数据前先清空
        self.data_list = {};
        self.data_obj = {};
        var $tbody = self.$el.find("tbody");
        // 添加加载loading
        $tbody.html(self._loadingTips(self.options.loadingTips));
        if (self.options.url) {
            self.trigger('loading');
            $.ajax({
                type: self.options.type,
                url: self.options.url,
                data: parms ? parms : self.options.ajaxParams,
                dataType: "JSON",
                success: function(data, textStatus, jqXHR) {
                    self.renderTable(data);
                    self.trigger('load-success', data);
                },
                error: function(res, textStatus) {
                    $tbody.html(self._loadingTips(res.responseText));
                    self.trigger('load-error', textStatus, res);
                },
            }).always(function(jqXhr, textStatus){         // 相当于 complete
                self.trigger('load-complete', textStatus, jqXhr);
            });
        } else {
            self.trigger('loading');
            if(self.options.data){
                self.renderTable(self.options.data);
                self.trigger('load-success', self.options.data);
            }else{
                self.trigger('load-error');
            }
            self.trigger('load-complete');
        }
    };
    
    BootstrapTreeTable.prototype._loadingTips = function(content) {
         return `<tr><td colspan="${this.options.columns.length}"><div class="tips">${content}</div></td></tr>`;
    };
    // 加载完数据后渲染表格
    BootstrapTreeTable.prototype.renderTable = function(data) {
        var self = this;
        var $tbody = self.$el.find("tbody");
        // 先清空
        $tbody.html("");
        if (!data || data.length <= 0) {
            $tbody.html(self._loadingTips(self.options.nonResultTips));
            return;
        }
        // 缓存并格式化数据
        self.formatData(data);
        // 获取所有根节点
        var rootNode = self.data_list["_root_"];
        // 开始绘制
        if (rootNode) {
            $.each(rootNode, function(i, item) {
                var _child_row_id = "row_id_" + i
                self.recursionNode(item, 1, _child_row_id, "row_root");
            });
        }
        // 下边的操作主要是为了查询时让一些没有根节点的节点显示
        $.each(data, function(i, item) {
            if (!item.isShow) {
                var tr = self.renderRow(item, false, 1, "", "");
                $tbody.append(tr);
            }
        });
        self.$el.append($tbody);
        self.initHiddenColumns();
        self.registerRowEvent();
        self.registerExpanderEvent();
        // 注册刷新按钮事件
        if (self.options.showRefresh) {
            self.registerRefreshBtnClickEvent();
        }
        // 注册刷新按钮事件
        if (self.options.showCheckAll) {
            self.registerCheckAllBtnClickEvent();
            self.registerUncheckAllBtnClickEvent();
        }
        // 注册展开所有按钮事件
        // 注册折叠所有按钮事件
        if (self.options.showExpandCollapse) {
            self.registerExpandBtnClickEvent();
            self.registerCollapseBtnClickEvent();
        }
        // 注册列选项事件
        if (self.options.showColumns) {
            self.registerColumnClickEvent();
        }
    };
    // 缓存并格式化数据
    BootstrapTreeTable.prototype.formatData = function(data) {
        var self = this;
        var _root = self.options.rootIdValue ? self.options.rootIdValue : null
        $.each(data, function(index, item) {
            // 添加一个默认属性，用来判断当前节点有没有被显示
            item.isShow = false;
            // 这里兼容几种常见Root节点写法
            // 默认的几种判断
            let _parentId = self.options.parentId;
            let _parentIdVal = item[_parentId];
            var _defaultRootFlag = _parentIdVal == '0' || _parentIdVal == 0 || _parentIdVal == null || _parentIdVal == '';
            if (!_parentIdVal || (_root ? (_parentIdVal == self.options.rootIdValue) : _defaultRootFlag)) {
                if (!self.data_list["_root_"]) {
                    self.data_list["_root_"] = [];
                }
                if (!self.data_obj["id_" + item[self.options.id]]) {
                    self.data_list["_root_"].push(item);
                }
            } else {
                if (!self.data_list["_n_" + _parentIdVal]) {
                    self.data_list["_n_" + _parentIdVal] = [];
                }
                if (!self.data_obj["id_" + item[self.options.id]]) {
                    self.data_list["_n_" + _parentIdVal].push(item);
                }
            }
            self.data_obj["id_" + item[self.options.id]] = item;
        });
    };
    // 递归获取子节点并且设置子节点
    BootstrapTreeTable.prototype.recursionNode = function(parentNode, lv, row_id, p_id) {
        var self = this;
        var $tbody = self.$el.find("tbody");
        var _ls = self.data_list["_n_" + parentNode[self.options.id]];
        var $tr = self.renderRow(parentNode, _ls ? true : false, lv, row_id, p_id);
        $tbody.append($tr);
        if (_ls) {
            $.each(_ls, function(i, item) {
                var _child_row_id = row_id + "_" + i
                self.recursionNode(item, (lv + 1), _child_row_id, row_id)
            });
        }
    };
    // 绘制行
    BootstrapTreeTable.prototype.renderRow = function(item, isP, lv, row_id, p_id) {
        var self = this;
        // 标记已显示
        item.isShow = true;
        item.row_id = row_id;
        item.p_id = p_id;
        item.lv = lv;
        var $tr = $('<tr rid="' + row_id + '" pid="' + p_id + '" dataid="' + item[self.options.id] + '"></tr>');
        var _icon = self.options.expanderCollapsedClass;
        if (self.options.expandAll) {
            $tr.css("display", "table");
            _icon = self.options.expanderExpandedClass;
        } else if (lv == 1) {
            $tr.css("display", "table");
            _icon = (self.options.expandFirst) ? self.options.expanderExpandedClass : self.options.expanderCollapsedClass;
        } else if (lv == 2) {
            $tr.css("display", self.options.expandFirst ? "table" : "none");
            _icon = self.options.expanderCollapsedClass;
        } else {
            $tr.css("display", "none");
            _icon = self.options.expanderCollapsedClass;
        }
        // 因有可能出现冻结列，所以这里合并列配置
        var _columns = self.leftFixedColumns.concat(self.noFixedColumns);
        $.each(_columns, function(index, column) {
            // 判断是不是选择列
            if (self._isCheckRadio(column)) {
                let width = column.width ? column.width : '30px'; // 默认宽度最小设置成30px
                
                var $td = $(`<td style="text-align:center;width:${width}"></td>`);
                if (column.checkbox && self.options.hierarchical) { // 如果有联级操作，则不能居中, 宽度设置为60px, 方便从左至右显示层次
                    width = column.width ? column.width : '60px';
                    if (lv > 1) { // 非顶层结点，向右偏移多少
                        $td = $(`<td style="width:${width};padding-right:0;padding-left:
                            ${(lv - 1)*self.options.hierarchicalCheckboxOffset-(lv>2?8:0)}px;"></td>`);
                    } else {
                        $td = $(`<td style="width:${width};"></td>`);
                    }
                }
                // 这里尝试几种参数去判断是否是选中
                let _isChecked = item.checked || item.selected || item.yes || item.flag;
                let _checked = "";
                if (_isChecked && _isChecked == true) {
                    _isChecked = true;
                    _checked = " checked"
                }
                let _value = item[self.options.id];
                $td.append($(`<input name="select_item" type="${column.radio ? 'radio' : 'checkbox'}"
                     value="${_value}"${_checked}></input>`));
                $tr.append($td);
                
                if (_isChecked && _value) {
                    _value = _value.toString();
                    // id加到管理器中，并在tr上加了 selected样式
                    if (column.radio) {
                        self.selectedDataIdsManager(_value);
                    } else {
                        self.selectedDataIdsManager(_value, true, true);
                    }
                    $tr.addClass('treetable-selected');
                }
            } else {
                // 给td增加样式, 方便美化和控制宽度
                let _cssClass = column.field + '_cls';
                _cssClass = column.cssClass ? column.cssClass + ' ' + _cssClass : column.field + '_cls';
                var $td = $('<td name="' + column.field + '" class="' + _cssClass + '"></td>');
                if (column.width) {
                    $td.css("width", column.width);
                }
                if (column.align) {
                    $td.css("text-align", column.align);
                }
                if (self.expandColumnField == column.field) {
                    $td.css("text-align", "left");
                }
                if (column.valign) {
                    $td.css("vertical-align", column.valign);
                }
                if (self.options.showTitle) {
                    $td.addClass("ellipsis");
                }
                // 增加formatter渲染
                if (column.formatter) {
                    $td.html(column.formatter.call(self, item[column.field], item, index));
                } else {
                    if (self.options.showTitle) {
                        // 只在字段没有formatter时才添加title属性
                        $td.attr("title", item[column.field]);
                    }
                    $td.text(item[column.field]);
                }
                if (self.expandColumnField == column.field) {
                    $td.prepend(`<span class="treetable-expander ${isP ? _icon : ''}"></span>`)
                    for (var int = 0; int < (lv - 1); int++) {
                        $td.prepend('<span class="treetable-indent"></span>')
                    }
                }
                $tr.append($td);
            }
        });
        return $tr;
    };
    BootstrapTreeTable.prototype._getToolbarTarget = function(targetClass) {
        let _rb = this.options.rightToolbar, $dom;
        targetClass = targetClass.startsWith('.') ? targetClass : '.' + targetClass;
        if (_rb) {
            $dom = $(_rb).find(targetClass);
        } else {
            $dom = $(".bootstrap-tree-table .treetable-bars .tool-right " + targetClass);
        }
        return $dom;
    };

    // 抽离统一按钮注册事件
    BootstrapTreeTable.prototype._registerToolbarBtnClickEvent = function(targetClass, callback) {
        var self = this;
        self._getToolbarTarget(targetClass).off('click').on('click', function() {
            callback.call(self, this);
        });
    };
    // 注册刷新按钮点击事件
    BootstrapTreeTable.prototype.registerRefreshBtnClickEvent = function() {
        var self = this;
        self._registerToolbarBtnClickEvent('refresh_btn', self._refresh);
    };
    // 注册展开折叠所有按钮点击事件
    BootstrapTreeTable.prototype.registerExpandBtnClickEvent = function() {
        var self = this;
        self._registerToolbarBtnClickEvent('expand_btn', self.expandAll);
    };
    BootstrapTreeTable.prototype.registerCollapseBtnClickEvent = function() {
        var self = this;
        self._registerToolbarBtnClickEvent('collapse_btn', self.collapseAll);
    };

    // 注册列选项事件
    BootstrapTreeTable.prototype.registerColumnClickEvent = function() {
        this._registerToolbarBtnClickEvent('columns label input', function(dom) {
            let _self = this, val = $(dom).val();
            if ($(dom).prop('checked')) {
                _self.showColumn(val);
            } else {
                _self.hideColumn(val);
            }
        });
    };
    // 注册全选中事件
    BootstrapTreeTable.prototype.registerCheckAllBtnClickEvent = function() {
        var self = this;
        self._registerToolbarBtnClickEvent('check_all_btn', function() {
            self.checkAll();
        });
    };
    // 注册取消全选中事件
    BootstrapTreeTable.prototype.registerUncheckAllBtnClickEvent = function() {
        var self = this;
        self._registerToolbarBtnClickEvent('uncheck_all_btn', function() {
            self.uncheckAll();
        });
    };
    // 注册行点击事件
    BootstrapTreeTable.prototype.registerRowEvent = function() {
        var self = this;
        let _container = self._getContainer();
        let _checkTarget = "input[name='select_item']";
        
        if (self.hasSelectItem) { // 如果有选择列才绑定事件
            let _chkbox = _container.find("tbody tr").find("td:first").find(_checkTarget);
            // 指定用来判断选中的目标
            
            // 如果有联级操作
            if (self.options.hierarchical) {
                // 绑定checkbox上的事件
                _chkbox.on('click', function(e) {
                    e.stopPropagation(); // 阻止冒泡, 不然会传递到td上，造成重复执行
                    
                    let val = $(this).val(), isChecked = $(this).is(':checked');
                    self._hierarchicalChildrenCheck(this, isChecked, _checkTarget);
        
                    // 先判断是否有兄弟结点选中, 如果有兄弟结点有选中，则不再向上递归
                    let item = self.data_obj["id_" + val];
                    if ($(`tr[pid="${item.p_id}"]`).find('td:first').find(`${_checkTarget}:checked`).not(this).length == 0) {
                        self._hierarchicalParentCheck(this, isChecked, _checkTarget);
                    } else {
                        self.selectedDataIdsManager(val, true, isChecked);
                    }
                    // 联级操作展开和折叠
                    if (isChecked && self.options.expandRowWhenChecked) {
                        self.expandRow(val);
                    }
                    if (!isChecked && self.options.collapseRowWhenUnchecked) {
                        self.collapseRow(val);
                    }
                });
            } 
        }
        _container.find("tbody tr td").off('click dblclick').on('click dblclick', function(e) {
            var $td = $(this),
                $tr = $td.parent(),
                dataid = $tr.attr("dataid"),
                item = self.data_obj["id_" + dataid],
                field = $td.attr("name"),
                value = item[field];
            // 如果第一列是选择列
            if (self.hasSelectItem && $td.index() == 0) {
                e.stopPropagation(); // 阻止冒泡
                 // 绑定td上的事件，点击td相当于点击了checkbox
                $td.find(_checkTarget).trigger('click');
                return;
            }
            
            // 如果此列是展开按钮列，则直接绑定toggleRow
            if ($td.index() == self.options.expandColumn) {
                self.toggleRow(dataid);
            }
            
            self.trigger(e.type === 'click' ? 'click-cell' : 'dbl-click-cell', field, value, item, $td);
            self.trigger(e.type === 'click' ? 'click-row' : 'dbl-click-row', item, $tr, field);

            if (!self.options.hierarchical) {
                let $ipt = $tr.find(_checkTarget);
                let _ipt_val = $ipt.val();
                if ($ipt.attr("type") == "radio") {
                    self.selectedDataIdsManager(_ipt_val);
                } else {
                    self.selectedDataIdsManager(_ipt_val, true, !$ipt.prop('checked'));
                }
            }
        });

        // 鼠标移入移出行事件
        if (self.options.hover) {
            let _cls = "treetable-hover";
            _container.find("tbody").on('mouseover', 'tr', function() { //鼠标移入行
                $(this).addClass(_cls)
            }).on('mouseout', 'tr', function() { //鼠标移出行
                $(this).removeClass(_cls)
            });
        }
    };
    // checkbox选中后联级操作-批量选中子结点
    BootstrapTreeTable.prototype._hierarchicalChildrenCheck = function(checkbox, isChecked, target) {
        var self = this;
         
        var $ckbox = $(checkbox);
        self.selectedDataIdsManager($ckbox.val(), true, isChecked);
        
        let $tr = $ckbox.closest('tr'), childNodes = self.data_list['_n_' + $tr.attr("dataid")];
        
        if (childNodes) {
             $.each(childNodes, function(i, item) {
                 let ck = $(`tr[rid="${item.row_id}"] td:first`).find(target);
                 self._hierarchicalChildrenCheck(ck, isChecked, target);
            });
        }
    };

    // checkbox选中后联级操作-批量选中父节点
    BootstrapTreeTable.prototype._hierarchicalParentCheck = function(checkbox, isChecked, target) {
        var self = this;
         
        var $ckbox = $(checkbox);
        self.selectedDataIdsManager($ckbox.val(), true, isChecked);
        
        let $tr = $ckbox.closest('tr'), item = self.data_obj["id_" + $tr.attr("dataid")];
        if (item && item.lv > 1) {
            let pck = $(`tr[rid="${item.p_id}"] td:first`).find(target);
            self._hierarchicalParentCheck(pck, isChecked, target);
        }
    };
    
    // 选择列管理者
    BootstrapTreeTable.prototype.selectedDataIdsManager = function(itemVal,isCheckbox,isAdd) {
        var self = this;
        var $ipt = self.$el.find("tbody tr").find("td:first").find("input[value='"+itemVal+"']");
        let _cls = "treetable-selected";
        if(isCheckbox){
            var _index = $.inArray(itemVal, self.selectedDataIds);
            if(isAdd && _index == -1) { // 不存在则添加，存在忽略
                self.selectedDataIds.push(itemVal);
                $ipt.closest('tr').addClass(_cls);
                $ipt.prop('checked', true);
            } 
            
            if (!isAdd && _index > -1) { // 存在的元素才删除掉, 否则忽略
                self.selectedDataIds.splice(_index, 1);
                $ipt.closest('tr').removeClass(_cls);
                $ipt.prop('checked', false);
            }
        } else {
            self.selectedDataIds=[];
            self.selectedDataIds.push(itemVal);
            self.$el.find("tbody tr").removeClass(_cls);
            $ipt.closest('tr').addClass(_cls);
            $ipt.prop('checked', true);
        }
    };
    // 注册小图标点击事件--展开缩起
    BootstrapTreeTable.prototype.registerExpanderEvent = function() {
        var self = this;
        self.$el.find("tbody tr").find(".treetable-expander").off('click').on('click', function(e) {
            e.stopPropagation(); // 阻止冒泡
            let me = $(this);
            var _isExpanded = me.hasClass(self.options.expanderExpandedClass);
            var _isCollapsed = me.hasClass(self.options.expanderCollapsedClass);
            if (_isExpanded || _isCollapsed) {
                self.toggleRow(me.closest('tr').attr("dataid"));
            }
        });
    };
    // 刷新数据， 内部使用
    BootstrapTreeTable.prototype._refresh = function(btn) {
        var self = this;
        self.destroy();
        self.init(self.lastAjaxParams);
    };
    // 刷新数据,外部api使用
    BootstrapTreeTable.prototype.refresh = function (params) {
        debugger;
        var self = this;
        self.destroy();
        if (params) {
            self.lastAjaxParams = params;
        }
        self.init(self.lastAjaxParams);
    };
    // 添加数据刷新表格
    BootstrapTreeTable.prototype.appendData = function(data) {
        var self = this;
        $.each(data, function(i, item) {
            var _data = self.data_obj["id_" + item[self.options.id]];
            var _p_data = self.data_obj["id_" + item[self.options.parentId]];
            var _c_list = self.data_list["_n_" + item[self.options.parentId]];
            var row_id = ""; //行id
            var p_id = ""; //父行id
            var _lv = 1; //如果没有父就是1默认显示
            var tr; //要添加行的对象
            if (_data && _data.row_id && _data.row_id != "") {
                row_id = _data.row_id; // 如果已经存在了，就直接引用原来的
            }
            if (_p_data) {
                p_id = _p_data.row_id;
                var _row_id_lastNum = 0
                if (row_id == "") {
                    if (_c_list && _c_list.length > 0) {
                        _row_id_lastNum = _c_list.length;
                    }
                    row_id = _p_data.row_id + "_" + _row_id_lastNum;
                }else{
                    var _tmp  = row_id.split("_");
                    _row_id_lastNum = _tmp[_tmp.length-1];
                }
                _lv = _p_data.lv + 1; //如果有父
                // 绘制行
                tr = self.renderRow(item, false, _lv, row_id, p_id);

                var _p_icon = self.$el.find("tr[rid='" + _p_data.row_id+"']").find(".treetable-expander");
                var _isExpanded = _p_icon.hasClass(self.options.expanderExpandedClass);
                var _isCollapsed = _p_icon.hasClass(self.options.expanderCollapsedClass);
                // 父节点有没有展开收缩按钮
                if (_isExpanded || _isCollapsed) {
                    // 父节点展开状态显示新加行
                    if (_isExpanded) {
                        tr.css("display", "table");
                    }
                } else {
                    // 父节点没有展开收缩按钮则添加
                    _p_icon.addClass(self.options.expanderCollapsedClass);
                }

                if (_data) {
                    self.$el.find("tr[rid='" + _data.row_id+"']").remove();
                }
                // 画上
                if(_row_id_lastNum==0){
                    self.$el.find("tr[rid='" + _p_data.row_id+"']").after(tr);
                }else{
                    self.$el.find("tr[rid='" + _p_data.row_id+"_"+(_row_id_lastNum-1)+"']").after(tr);
                }
            } else {
                tr = self.renderRow(item, false, _lv, row_id, p_id);
                if (_data) {
                    var $prev = self.$el.find("tr[rid='" + _data.row_id+"']").prev();
                    self.$el.find("tr[rid='" + _data.row_id+"']").remove();
                    $prev.after(tr);
                } else {
                    // 画上
                    var tbody = self.$el.find("tbody");
                    tbody.append(tr);
                }
            }
            item.isShow = true;
            // 缓存并格式化数据
            self.formatData([item]);
        });
        self.initHiddenColumns();
        self.registerRowEvent();
        self.registerExpanderEvent();
    };
    // 展开/折叠指定的行
    BootstrapTreeTable.prototype.toggleRow = function(id) {
        var self = this;
        var _rowData = self.data_obj["id_" + id];
        var $tr = self.$el.find("tr[rid='" + _rowData.row_id+ "']");
        var row_id = $tr.attr("rid");
        var $row_expander = $tr.find(".treetable-expander");
        var _isExpanded = $row_expander.hasClass(self.options.expanderExpandedClass);
        var _isCollapsed = $row_expander.hasClass(self.options.expanderCollapsedClass);
        if (_isExpanded || _isCollapsed) {
            var _ls = self.$el.find("tbody").find("tr[rid^='" + row_id + "_']"); //下所有
            if (_isExpanded) {
                $row_expander.removeClass(self.options.expanderExpandedClass);
                $row_expander.addClass(self.options.expanderCollapsedClass);
                if (_ls && _ls.length > 0) {
                    $.each(_ls, function(index, item) {
                        $(item).css("display", "none");
                    });
                }
            } else {
                $row_expander.removeClass(self.options.expanderCollapsedClass);
                $row_expander.addClass(self.options.expanderExpandedClass);
                if (_ls && _ls.length > 0) {
                    $.each(_ls, function(index, item) {
                        // 父icon
                        var _p_icon = $("tr[rid='" + $(item).attr("pid")+"']").find(".treetable-expander");
                        if (_p_icon.hasClass(self.options.expanderExpandedClass)) {
                            $(item).css("display", "table");
                        }
                    });
                }
            }
        }
    };
    // 展开指定的行
    BootstrapTreeTable.prototype.expandRow = function(id) {
        var self = this;
        var _rowData = self.data_obj["id_" + id];
        var $tr = self.$el.find("tr[rid='" + _rowData.row_id+ "']");
        var $row_expander = $tr.find(".treetable-expander");
        var _isExpanded = $row_expander.hasClass(self.options.expanderExpandedClass);
        var _isCollapsed = $row_expander.hasClass(self.options.expanderCollapsedClass);
        if (_isExpanded || _isCollapsed) {
            if (_isCollapsed) {
                self.toggleRow(id)
            }
        }
    };

    // 折叠 指定的行
    BootstrapTreeTable.prototype.collapseRow = function(id) {
        var self = this;
        var _rowData = self.data_obj["id_" + id];
        var $tr = self.$el.find("tr[rid='" + _rowData.row_id+ "']");
        var $row_expander = $tr.find(".treetable-expander");
        var _isExpanded = $row_expander.hasClass(self.options.expanderExpandedClass);
        var _isCollapsed = $row_expander.hasClass(self.options.expanderCollapsedClass);
        if (_isExpanded || _isCollapsed) {
            if (_isExpanded) {
                self.toggleRow(id)
            }
        }
    };
    // 展开所有的行
    BootstrapTreeTable.prototype.expandAll = function(btn) {
        this._toggleRowAll(this.options.expanderCollapsedClass);
    };
    // 折叠所有的行
    BootstrapTreeTable.prototype.collapseAll = function(btn) {
        this._toggleRowAll(this.options.expanderExpandedClass);
    };
    // 展开折叠所有的行
    BootstrapTreeTable.prototype._toggleRowAll = function(cls) {
        var self = this, _cls=cls.replace(/\s+/g, '.');
        self.$el.find("tbody tr").find(".treetable-expander." + _cls).each(function(i, n) {
            self.toggleRow($(n).closest('tr').attr("dataid"));
        })
    };
    // 选中所有记录
    BootstrapTreeTable.prototype.checkAll = function() {
        this._toggleCheckAll(true);
    };
    // 取消选中所有记录
    BootstrapTreeTable.prototype.uncheckAll = function() {
        this._toggleCheckAll(false);
    };
    // 选中、取消选中所有记录
    BootstrapTreeTable.prototype._toggleCheckAll = function(isCheckAll) {
        var self = this;
        let _checkTarget = self.hasSelectItem ? "input[name='select_item']" : "input[type='checkbox']",
            _container = self._getContainer(),
            $domTrs = _container.find("tbody tr"),
            $chks = $domTrs.find("td:first").find(_checkTarget);
        // 先判断数量，已全选和未全选直接跳过，不再执行后面的代码
        let _checkedNums = $chks.find(':checked');
        if ((isCheckAll && _checkedNums == $chks.length) || (!isCheckAll && _checkedNums == 0)) {
            return;
        }
        $chks.prop('checked', isCheckAll);
        // 加到id管理器中
        let _cls = "treetable-selected";
        if (isCheckAll) {
            $domTrs.addClass(_cls)
            var ids = [];
            $chks.each(function(idx, dom) {
                ids.push($(dom).val());
            });
            self.selectedDataIds=ids;
        } else {
            self.selectedDataIds=[];
            $domTrs.removeClass(_cls)
        }
    };
    // 显示指定列
    BootstrapTreeTable.prototype.showColumn = function(field) {
        var self = this;
        // 固定列不让隐藏
        $.each(self.leftFixedColumns, function(i, column) {
            if (column.field == field) {
                return;
            }
        });
        var _index = $.inArray(field, self.hiddenColumns);
        if (_index > -1) {
            self.hiddenColumns.splice(_index, 1);
        }
        self._getContainer().find("." + field + "_cls").show();
        //是否更新列选项状态
        if (self.options.showColumns) {
            let $dom = self._getToolbarTarget('columns label');
            $dom.find("input[value='" + field + "']").prop("checked", true);
        }
    };
    // 隐藏指定列
    BootstrapTreeTable.prototype.hideColumn = function(field) {
        var self = this;
        // 固定列不让隐藏
        $.each(self.leftFixedColumns, function(i, column) {
            if (column.field == field) {
                return;
            }
        });
        self.hiddenColumns.push(field);
        self._getContainer().find("." + field + "_cls").hide();
        //是否更新列选项状态
        if (self.options.showColumns) {
            let $dom = self._getToolbarTarget('columns label');
            $dom.find("input[value='" + field + "']").prop("checked", false);
        }
    };
    // 获取已选行
    BootstrapTreeTable.prototype.getSelections = function() {
        var self = this;
        if (self.selectedDataIds) {
            var chk_value = [];
            $.each(self.selectedDataIds, function(i, id) {
                chk_value.push(self.data_obj["id_" + id]);
            });
            return chk_value;
        }
        return;
    };
    // 获取已选行ID列表
    BootstrapTreeTable.prototype.getSelectionIds = function() {
        return this.selectedDataIds;
    };
    // 触发事件
    BootstrapTreeTable.prototype.trigger = function(name) {
        var self = this;
        var args = Array.prototype.slice.call(arguments, 1);

        let suffix = '.bs.tree.table';
        name += suffix;
        self.options[BootstrapTreeTable.EVENTS[name]].apply(self.options, args);
        self.$el.trigger($.Event(name), args);

        self.options.onAll(name, args);
        self.$el.trigger($.Event('all' + suffix), [name, args]);
    };
    // 销毁
    BootstrapTreeTable.prototype.destroy = function() {
        var self = this;
        var $container = self._getContainer();
        self.$el.insertBefore($container);
        $(self.options.toolbar).insertBefore(self.$el);
        self.$el.html(self.$el_.html());
        $container.remove();
        self.$headerBox = null;
        self.$leftBox = null;
        self.selectedDataIds = [];
        self.hiddenColumns = [];
        self.leftFixedColumns = [];
        self.noFixedColumns = [];
        self.data_list = null;
        self.data_obj = null;
        self.lastAjaxParams = null;
        self.hasSelectItem = false;
        self.expandColumnIsFixed = false;
        // add by leo.du
        if (self.options.rightToolbar) {
            $(self.options.rightToolbar).find('.tool-right').remove();
        }
    };

    // 组件方法
    BootstrapTreeTable.METHODS = [
        "getSelections",
        "getSelectionIds",
        "refresh",
        "appendData",
        "toggleRow",
        "expandRow",
        "collapseRow",
        "checkAll",
        "uncheckAll",
        "expandAll",
        "collapseAll",
        "showColumn",
        "hideColumn",
        "destroy"
    ];

    // 组件事件
    BootstrapTreeTable.EVENTS = {
        'all.bs.tree.table': 'onAll',
        'click-cell.bs.tree.table': 'onClickCell',
        'dbl-click-cell.bs.tree.table': 'onDblClickCell',
        'click-row.bs.tree.table': 'onClickRow',
        'dbl-click-row.bs.tree.table': 'onDblClickRow',
        'load-success.bs.tree.table': 'onLoadSuccess',
        'load-error.bs.tree.table': 'onLoadError',
        // add by leo.du, 用来支持加载动画或者遮罩动画
        'loading.bs.tree.table': 'onLoading',
        'load-complete.bs.tree.table': 'onLoadComplete'
    };
    // 默认配置
    BootstrapTreeTable.DEFAULTS = {
        id: 'id', // 选取记录返回的值,用于设置父子关系
        parentId: 'parentId', // 用于设置父子关系
        rootIdValue: null, // 设置根节点id值----可指定根节点，默认为null,"",0,"0"
        data: null, // 构造table的数据集合
        type: "GET", // 请求数据的ajax类型
        url: null, // 请求数据的ajax的url
        ajaxParams: {}, // 请求数据的ajax的data属性
        //lazzyLoad: false, // 是否异步加载每个node结点, 未实现
        expandColumn: 0, // 在哪一列上面显示展开按钮, 从0开始, 0是第1列
        expandAll: false, // 是否全部展开
        expandFirst: true, // 是否默认第一级展开--expandAll为false时生效
        striped: false, // 是否各行渐变色, 不建议使用
        bordered: true, // 是否显示边框
        hover: true, // 是否鼠标悬停
        condensed: false, // 是否紧缩表格
        columns: [], // 列
        toolbar: null, // 顶部工具条
        // 工具条按钮的样式，默认是btn-default btn-sm, 可以自己添加样式或者选择内置的btn-success btn-xs, btn-primary等组合样式
        toolbarBtnStyle: 'btn-default btn-sm',
        rightToolbar: null, // 顶部右侧工具条, 可以指定位置，而不是之前源码里面固定位置, 指定此项toolbar选项失效
        width: 0, // 表格宽度
        height: 0, // 表格高度
        showTitle: true, // 是否采用title属性显示字段内容（被formatter格式化的字段不会显示）
        showColumns: true, // 是否显示内容列下拉框
        showRefresh: true, // 是否显示刷新按钮
        showExpandCollapse: true, // 是否显示展开折叠所有按钮
        showBatchSort: false, // 是否显示批量排序,这个需要程序内有排序的功能, 事件需要自己注册实现
        showCheckAll: false, // 是否显示批量选中和批量取消选中
        expanderExpandedClass: 'ri-subtract-fill', // 展开的按钮的图标
        expanderCollapsedClass: 'ri-add-fill', // 缩起的按钮的图标
        toolRefreshClass: 'fa fa-refresh', // 工具栏刷新按钮
        toolColumnsClass: 'fa fa-table', // 工具栏列按钮
        toolExpandClass: 'fa fa-plus-square', // 工具栏展开所有按钮
        toolCollapseClass: 'fa fa-minus-square', // 工具栏折叠所有按钮
        toolBatchSortClass: 'fa fa-sort', // 工具栏批量排序按钮
        toolCheckAllClass: 'fa fa-check-square-o', // 工具栏批量排序按钮
        toolUncheckAllClass: 'fa fa-square-o', // 工具栏批量排序按钮
        loadingTips: '正在努力地加载数据中，请稍候……', // ajax加载动画或者文字，也可以是html代码
        nonResultTips: '没有找到匹配的记录', // 没有加载到记录时显示的文字，也可以是html代码
        hierarchical:true, // 联级checkbox, 选中上级或者下级自动关联
        hierarchicalCheckboxOffset: 22, // 联级checkbox时，子结点checkbox向右的偏移量， 单位: px
        expandRowWhenChecked: true,  // 联级选中checkbox时，是否展开当前行 TODO 可以更进一步展开所有子结点，这个比较激进
        collapseRowWhenUnchecked: false, // 联级取消选中checkbox时，是否折叠当前行
        onAll: function(data) {
            return false;
        },
        onLoading: function() {
            return false;
        },
        onLoadSuccess: function(data) {
            return false;
        },
        onLoadError: function(status) {
            return false;
        },
        onLoadComplete: function(status, jqXhr) {
            return false;
        },
        onClickCell: function(field, value, row, $element) {
            return false;
        },
        onDblClickCell: function(field, value, row, $element) {
            return false;
        },
        onClickRow: function(row, $element) {
            return false;
        },
        onDblClickRow: function(row, $element) {
            return false;
        }
    };

    BootstrapTreeTable.COLUMN_DEFAULTS = {
        radio: false,
        checkbox: false,
        field: undefined,
        title: undefined,
        align: undefined, // left, right, center
        valign: undefined, // top, middle, bottom
        width: undefined,
        visible: true,
        fixed:undefined,//固定列。可选值有：left（固定在左）。一旦设定，对应的列将会被固定在左，不随滚动条而滚动。
        formatter: undefined,
        cssClass: undefined // 单元格样式
    };

    $.fn.bootstrapTreeTable = function(option) {
        var value, args = Array.prototype.slice.call(arguments, 1);
        this.each(function() {
            var _self = $(this),
                data = _self.data(pluginName),
                options = $.extend({}, BootstrapTreeTable.DEFAULTS, _self.data(),
                    typeof option === 'object' && option);
            if (typeof option === 'string') {
                if ($.inArray(option, BootstrapTreeTable.METHODS) < 0) {
                    throw new Error("Unknown method: " + option);
                }
                if (!data) {
                    return;
                }
                value = data[option].apply(data, args);
                if (option === 'destroy') {
                    _self.removeData(pluginName);
                }
            }
            if (!data) {
                _self.data(pluginName, (data = new BootstrapTreeTable(this, options)));
            }
        });
        return typeof value === 'undefined' ? this : value;
    };
})(jQuery);