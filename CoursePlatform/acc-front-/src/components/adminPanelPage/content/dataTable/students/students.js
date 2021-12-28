import React from 'react';
import { Button, Space, Table, Input } from 'antd';
import { FormOutlined, CloseOutlined } from '@ant-design/icons';
import usersService from '../../../../../services/students';
import { modalsTypes } from '../../../../modal/modalsTypes';
import { alertTypes } from '../../../../alert/types';
import PagePagination from "../../../../pagination/index";
import moment from 'moment';
import ElementsOnPage from '../../../../elementsOnPage/index';
import Highlighter from 'react-highlight-words';
import { SearchOutlined } from '@ant-design/icons';

const { Search } = Input;

class Students extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            students: [],
            dateFormat: "YYYY-MM-DD",

            elementsOnPage: this.props.elementsOnPage,
            currentPage: this.props.currentPage,
            isSortingChanged: this.props.isSortingChanged,

            searchText: undefined,
            searchOn: undefined
        };
    }

    componentDidMount() {
        document.title = "Students";
        
        this.getStudents();
    }

    componentDidUpdate() {
        
        if (this.state.isSortingChanged) {
            
            this.getStudents(this.state.searchText);
        }
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            students: nextProps.data,
            isSortingChanged: nextProps.isSortingChanged,
            currentPage: nextProps.currentPage,
            elementsOnPage: nextProps.elementsOnPage
        }
    }

    getStudents(searchText) {

        const {
            startLoading,
            finishLoading,
            setStudents,
            clearTotalCount,
            resetIsSortChangedStatus,
            setTotalCount
        } = this.props;

        if (this.state.isSortingChanged) {

            resetIsSortChangedStatus();
        }

        startLoading();

        var searchOn;

        switch (this.state.searchOn) {
            case "name":
                searchOn = 0;
                break;
            case "surname":
                    searchOn = 1;
                break;
            default:
                searchOn = 0;
                break;
        }

        var model = {
            "searchText": searchText,
            "searchOn": searchOn,
            "filterQuery": {
                "pageNumber": this.state.currentPage,
                "elementsOnPage": this.state.elementsOnPage,
                "sortDirection": 2,
                "sortBy": 4
            }
        }
        
        usersService.getStudents(model)
            .then((response) => {

                var studentKey = 0;

                response.data.students.map((info, index) => {

                    info.key = studentKey;
                    info.isEmailConfirmed = info.isEmailConfirmed.toString();
                    info.subscriptionsCount = info.subscriptions.length;
                    info.age = parseInt(moment.duration(moment().diff(new Date(info.birthday))).asYears(), 10);

                    if (info.age < 14) {
                        info.age = -1
                    };

                    var subscriptionKey = 1;

                    info.subscriptions.map((course, index) => {

                        course.key = subscriptionKey;
                        subscriptionKey = subscriptionKey + 1;
                    })

                    studentKey = studentKey + 1;
                })

                console.log();
                setStudents(response.data.students);
                setTotalCount(response.data.totalCount);
            },
                err => {

                    clearTotalCount();
                    this.setWarningAlert();
                })
            .catch(err => {

                clearTotalCount();
                this.setWarningAlert();
            })
            .finally(() => {
                finishLoading();
            });
    }

    setWarningAlert = () => {

        const {
            setAlert
        } = this.props;

        var model = {
            type: alertTypes.WARNING,
            message: "Something went wrong, try again !"
        }

        setAlert(model);
    }

    openModal = (record, type) => {
        const {
            openModal
        } = this.props;

        openModal({ type: type, info: record });
    }

    getColumnSearchProps = dataIndex => ({
        filterDropdown: ({ setSelectedKeys, selectedKeys, confirm, clearFilters }) => (
            <div style={{ padding: 8 }}>
                <Input
                    ref={node => {
                        this.searchInput = node;
                    }}
                    placeholder={`Search ${dataIndex}`}
                    value={selectedKeys[0]}
                    onChange={e => setSelectedKeys(e.target.value ? [e.target.value] : [])}
                    onPressEnter={() => this.handleSearch(selectedKeys, confirm, dataIndex)}
                    style={{ marginBottom: 8, display: 'block' }}
                />
                <Space>
                    <Button
                        type="primary"
                        onClick={() => this.handleSearch(selectedKeys, confirm, dataIndex)}
                        icon={<SearchOutlined />}
                        size="small"
                        style={{ width: 90 }}
                    >
                        Search
                    </Button>
                    <Button onClick={() => this.handleReset(clearFilters)} size="small" style={{ width: 90 }}>
                        Reset
                    </Button>
                    <Button
                        type="link"
                        size="small"
                        onClick={() => {
                            confirm({ closeDropdown: false });
                            this.setState({
                                searchText: selectedKeys[0],
                                searchedColumn: dataIndex,
                            });
                        }}
                    >
                        Filter
                    </Button>
                </Space>
            </div>
        ),
        filterIcon: filtered => <SearchOutlined style={{ color: filtered ? '#1890ff' : undefined }} />,
        onFilter: (value, record) =>
            record[dataIndex]
                ? record[dataIndex].toString().toLowerCase().includes(value.toLowerCase())
                : '',
        onFilterDropdownVisibleChange: visible => {
            if (visible) {
                setTimeout(() => this.searchInput.select(), 100);
            }
        }
    });

    handleSearch = (selectedKeys, confirm, dataIndex) => {
        
        confirm();        

        const {
            changeCurrentPage
        } = this.props;
        
        this.setState({
            searchText: selectedKeys[0],
            searchOn: dataIndex
        })

        changeCurrentPage(1);
    };

    handleReset = clearFilters => {
        
        clearFilters();

        this.setState({
            searchText: ""
        })

        this.getStudents();
    };

    render() {

        const columns = [
            {
                key: '1',
                title: 'Name',
                dataIndex: 'name',
                // sorter: (a, b) => a.name.localeCompare(b.name)
                ...this.getColumnSearchProps('name'),
            },
            {
                key: '2',
                title: 'Surname',
                dataIndex: 'surname',
                ...this.getColumnSearchProps('surname'),
            },
            {
                key: '3',
                title: 'Email',
                dataIndex: 'email',
            },
            {
                key: '4',
                title: 'Age',
                dataIndex: 'age'
            },
            {
                key: '5',
                dataIndex: 'isEmailConfirmed',
                title: 'Is email confirmed'
            },
            {
                key: '6',
                title: 'Subs. count',
                dataIndex: 'subscriptionsCount'
            },
            {
                title: 'Actions',
                dataIndex: '',
                key: 'x',
                render: (_, record) =>
                    <Space>
                        <Button type="primary" icon={<FormOutlined />}
                            className="edit-form-button"
                            style={{ backgroundColor: "orange" }}
                            onClick={() => this.openModal(record, modalsTypes.EDIT_STUDENT)}>
                            Edit
                        </Button>
                        <Button type="primary" icon={<CloseOutlined />}
                            className="edit-form-button" danger
                            onClick={() => this.openModal(record, modalsTypes.REMOVE_STUDENT)}>
                            Remove
                        </Button>
                    </Space>
            }
        ];

        const coursesColumns = [
            {
                key: '1',
                title: 'Id',
                dataIndex: 'key',
            },
            {
                key: '2',
                title: 'Title',
                dataIndex: 'title',
            },
            {
                key: '3',
                title: 'Description',
                dataIndex: 'description',
            }
        ]

        const tableProps = {
            expandedRowRender: record => (
                <Table columns={coursesColumns} dataSource={record.subscriptions} pagination={false} />
            ),
            rowExpandable: record => record.subscriptionsCount > 0
        };

        return (
            <>
                <ElementsOnPage />

                <PagePagination />

                <Table
                    {...tableProps}
                    columns={columns}
                    dataSource={this.state.students}
                    pagination={false}
                    style={{ "marginTop": "20px" }}
                />

                <PagePagination />
            </>
        );
    }
}

export default Students;