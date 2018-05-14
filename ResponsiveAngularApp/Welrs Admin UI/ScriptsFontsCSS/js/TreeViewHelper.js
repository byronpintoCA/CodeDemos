var _TheTreeView;
var _TheTreeViewName;
var _TheTreeViewNameNodeSelected;
var _TheTreeViewNameNodeUnSelected;
var _TheTreeViewNameNodeExpanded;
var _TheTreeViewNameNodeCollapsed;

var _lastSelectedNode;
var _lastScrollTop;

function TreeViewLoad(strTreeViewName, dataProvider, reSelect, funcOnNodeSelected, funcOnNodeUnselected, funcOnNodeExpanded, funcOnNodeCollapsed) {

    _TheTreeViewName = strTreeViewName;

    _TheTreeView = $(strTreeViewName).treeview({
        data: dataProvider(),
        multiSelect: false,
        //nodeIcon: 'glyphicon glyphicon-bookmark',
        color: "black",
        backColor: "#fffff",
        onhoverColor: "darkgray",
        borderColor: "#d9d9d9",
        selectedColor: "#111111",
        selectedBackColor: "LightBlue",
        showBorder: true,
        showTags: true,
        highlightSelected: true,
        //levels: 1,
        onNodeSelected: function (event, node) {
            _lastSelectedNode = node;
            _lastScrollTop = $(strTreeViewName).scrollTop();
            if (_TheTreeViewNameNodeSelected !== null) {
                _TheTreeViewNameNodeSelected(event, node);
            }
        },
        onNodeUnselected: function (event, node) {
            if (_TheTreeViewNameNodeUnSelected !== null) {
                _TheTreeViewNameNodeUnSelected(event, node);
            }
        },
        onNodeCollapsed: function (event, node) {
            if (_TheTreeViewNameNodeCollapsed !== null) {
                _TheTreeViewNameNodeCollapsed(event, node);
            }
        }

        ,
        onNodeExpanded: function (event, node) {
            if (_TheTreeViewNameNodeExpanded !== null) {
                _TheTreeViewNameNodeExpanded(event, node);
            }
        }
    });

    if (_lastSelectedNode !== null && reSelect === true) {
        $(strTreeViewName).treeview('selectNode', [_lastSelectedNode.nodeId, {
            silent: true
        }]);

        // Reset to Last Scroll Location
        $(strTreeViewName).scrollTop(_lastScrollTop);

        //$(strTreeViewName).treeview('revealNode', [ _lastSelectedNode.nodeId, { silent: true } ]);

    } else {
        $(strTreeViewName).scrollTop(0);
        _lastSelectedNode = null;
        _TheTreeViewNameNodeSelected = funcOnNodeSelected;
        _TheTreeViewNameNodeUnSelected = funcOnNodeUnselected;
        _TheTreeViewNameNodeExpanded = funcOnNodeExpanded;
        _TheTreeViewNameNodeCollapsed = funcOnNodeCollapsed;
    }
}

function TreeViewRemove() {
    $(_TheTreeViewName).treeview('remove');
}

function TestDataProvider() {

    var nodeData = [{
            text: 'Parent 1',
            tags: ['Ding'],
            type: 1,
            icon: 'glyphicon glyphicon-book',
            nodes: [{
                    text: 'Child 1',
                    tags: '',
                    type: 2,
                    icon: 'glyphicon glyphicon-list-alt',
                    nodes: [{
                            text: 'Grandchild 1',
                            tags: ['0'],
                            type: 3,
                            icon: 'glyphicon glyphicon-bookmark',

                        },
                        {
                            text: 'Grandchild 2',
                            tags: ['0'],
                            type: 3,
                            icon: 'glyphicon glyphicon-bookmark',
                        }
                    ]
                },
                {
                    text: 'Child 2',
                    tags: ['0'],
                    type: 2,
                }
            ]
        },
        {
            text: 'Parent 2',
            tags: ['0'],
            type: 1,
            icon: 'glyphicon glyphicon-certificate'
        },
        {
            text: 'Parent 3',
            tags: ['0'],
            type: 1,
        },
        {
            text: 'Parent 4',
            tags: ['0'],
            type: 1,
        },
        {
            text: 'Parent 5',
            tags: ['0'],
            type: 1,
            icon: 'glyphicon glyphicon-earphone'
        },
        // Test
        {
            text: 'Parent 1',
            tags: ['4'],
            type: 1,
            icon: 'glyphicon glyphicon-user',
            nodes: [{
                    text: 'Child 1',
                    tags: ['2'],
                    type: 2,
                    nodes: [{
                            text: 'Grandchild 1',
                            tags: ['0'],
                            type: 3,
                        },
                        {
                            text: 'Grandchild 2',
                            tags: ['0'],
                            type: 3,
                        }
                    ]
                },
                {
                    text: 'Child 2',
                    tags: ['0'],
                    type: 1,
                }
            ]
        },
        {
            text: 'Parent 2',
            tags: ['0'],
            type: 1,
            icon: 'glyphicon glyphicon-certificate'
        },
        {
            text: 'Parent 3',
            tags: ['0'],
            type: 1,
        },
        {
            text: 'Parent 4',
            tags: ['0'],
            type: 1,
        },
        {
            text: 'Parent 5',
            tags: ['0'],
            type: 1,
            icon: 'glyphicon glyphicon-earphone'
        },
        {
            text: 'Parent 1',
            tags: ['4'],
            type: 1,
            icon: 'glyphicon glyphicon-user',
            nodes: [{
                    text: 'Child 1',
                    tags: ['2'],
                    type: 2,
                    nodes: [{
                            text: 'Grandchild 1',
                            tags: ['0'],
                            type: 3,
                        },
                        {
                            text: 'Grandchild 2',
                            tags: ['0'],
                            type: 3,
                        }
                    ]
                },
                {
                    text: 'Child 2',
                    tags: ['0'],
                    type: 2,
                }
            ]
        },
        {
            text: 'Parent 2',
            tags: ['0'],
            type: 1,
            icon: 'glyphicon glyphicon-certificate'
        },
        {
            text: 'Parent 3',
            tags: ['0'],
            type: 1,
        },
        {
            text: 'Parent 4',
            tags: ['0'],
            type: 1,
        },
        {
            text: 'Parent 5',
            tags: ['0'],
            type: 1,
            icon: 'glyphicon glyphicon-earphone'
        }
        //ENd
    ];

    return nodeData;

}

/* http://jonmiles.github.io/bootstrap-treeview/ 
view-source:http://jonmiles.github.io/bootstrap-treeview/
https://github.com/jonmiles/bootstrap-treeview
 */