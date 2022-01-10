import React from 'react';
import { Button, Space, Table, Input } from 'antd';
import { FormOutlined, CloseOutlined } from '@ant-design/icons';
import usersService from '../../../../../services/students';
import { modalsTypes } from '../../../../modal/modalsTypes';
import { alertTypes } from '../../../../alert/types';
import { elementsOnPage } from '../../../../../constants/elementsOnPageCount';
import { sortByTypes } from '../../../../../constants/sortByTypes';
import { sortDirectionTypes } from '../../../../../constants/sortDirectionTypes';
import moment from 'moment';

const { Search } = Input;

class Students extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            students: [],
            dateFormat: "YYYY-MM-DD",

            elementsOnPage: elementsOnPage[10],
            sortBy: sortByTypes.REGISTEREDDATE,
            sortDirection: sortDirectionTypes.ASC,
            isSortingChanged: this.props.isSortingChangedRedux,
            totalCount: 0,
            currentPage: 1,
            searchText: undefined
        };
    }

    componentDidMount() {
        document.title = "Students";

        this.getStudents();
    }

    componentDidUpdate() {

        if (this.state.isSortingChanged) {

            this.getStudents();
        }
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            students: nextProps.data,
            isSortingChangedRedux: nextProps.isSortingChanged
        }
    }

    getStudents() {

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

        var model = {
            "searchText": this.state.searchText,
            "filterQuery": {
                "pageNumber": this.state.currentPage,
                "elementsOnPage": this.state.elementsOnPage,
                "sortDirection": this.state.sortDirection,
                "sortBy": this.state.sortBy
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
                        info.age = "not specified"
                    };

                    var subscriptionKey = 1;

                    info.subscriptions.map((course, index) => {

                        course.key = subscriptionKey;
                        subscriptionKey = subscriptionKey + 1;
                    })

                    studentKey = studentKey + 1;
                })

                this.setState({
                    totalCount: response.data.totalCount
                })

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

    handlePageChange = (page, pageSize) => {

        if (pageSize != this.state.elementsOnPage) {

            this.setState({
                currentPage: 1,
                elementsOnPage: pageSize
            }, function () { this.getStudents() });
        }
        else {
            this.setState({
                currentPage: page,
                elementsOnPage: pageSize
            }, function () { this.getStudents() });
        }
    }

    onSearch = (value) => {

        if (value != this.state.searchText) {

            this.setState({
                searchText: value,
                currentPage: 1
            }, function () { this.getStudents() });
        }
    }

    sortBy = (pagination, filters, sorter) => {

        var order;
        var field;

        switch (sorter.order) {
            case "descend": {
                order = sortDirectionTypes.DESC;
                break;
            }
            case "ascend": {
                order = sortDirectionTypes.ASC;
                break;
            }
            default: {
                this.setState({
                    sortBy: sortByTypes.REGISTEREDDATE,
                    sortDirection: sortDirectionTypes.ASC
                }, function () { this.getStudents() });
                return;
            }
        }

        switch (sorter.field) {
            case "name":
                {
                    field = sortByTypes.NAME;
                    break;
                }
            case "surname":
                {
                    field = sortByTypes.SURNAME;
                    break;
                }
            case "email":
                {
                    field = sortByTypes.EMAIL;
                    break;
                }
            case "age":
                {
                    field = sortByTypes.AGE;
                    break;
                }
        }

        this.setState({
            sortBy: field,
            sortDirection: order
        }, function () { this.getStudents() });
    }

    render() {

        const columns = [
            {
                key: '1',
                title: 'Name',
                dataIndex: 'name',
                sorter: true
            },
            {
                key: '2',
                title: 'Surname',
                dataIndex: 'surname',
                sorter: true
            },
            {
                key: '3',
                title: 'Email',
                dataIndex: 'email',
                sorter: true
            },
            {
                key: '4',
                title: 'Age',
                dataIndex: 'age',
                sorter: true
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

                <Search
                    placeholder="input search text"
                    allowClear
                    enterButton="Search"
                    size="large"
                    onSearch={(value) => this.onSearch(value)}
                />

                <Table
                    {...tableProps}
                    columns={columns}
                    dataSource={this.state.students}
                    showSorterTooltip={true}
                    pagination={{
                        position: "bottomRight",
                        current: this.state.currentPage,
                        total: this.state.totalCount,
                        onChange: (page, pageSize) => this.handlePageChange(page, pageSize)
                    }}
                    onChange={(pagination, filters, sorter) => this.sortBy(pagination, filters, sorter)}
                    style={{ "marginTop": "20px" }}
                />
            </>
        );
    }
}

export default Students;