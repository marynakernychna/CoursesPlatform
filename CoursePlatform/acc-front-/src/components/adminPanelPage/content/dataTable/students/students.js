import React from 'react';
import { Button, Space, Table, Input } from 'antd';
import { FormOutlined, CloseOutlined } from '@ant-design/icons';
import usersService from '../../../../../services/students';
import { modalsTypes } from '../../../../modal/modalsTypes';
import { alertTypes } from '../../../../alert/types';
import PagePagination from "../../../../pagination/index";
import moment from 'moment';
import ElementsOnPage from '../../../../elementsOnPage/index';

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

            searchText: ""
        };
    }

    componentDidMount() {
        document.title = "Students";

        var model =
        {
            "pageNumber": this.state.currentPage,
            "elementsOnPage": this.state.elementsOnPage
        }

        this.getStudents(model);
    }

    componentDidUpdate() {

        if (this.state.isSortingChanged) {

            var model;

            if (this.state.searchText == "") {

                model =
                {
                    "pageNumber": this.state.currentPage,
                    "elementsOnPage": this.state.elementsOnPage
                }

                this.getStudents(model);
            }
            else {

                this.onSearch(this.state.searchText);
            }
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

    getStudents(model) {

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

            this.setState({
                isSortingChanged: false
            })
        }

        startLoading();

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

    // modals actions

    openModal = (record, type) => {
        const {
            openModal
        } = this.props;

        openModal({ type: type, info: record });
    }

    onSearch = (searchText) => {

        if (searchText == "") {
            return;
        }

        this.setState({
            searchText: searchText
        });

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

            this.setState({
                isSortingChanged: false
            })
        }

        startLoading();

        var model = {
            "searchText": searchText,
            "studentsOnPageRequest": {
                "pageNumber": this.state.currentPage,
                "elementsOnPage": this.state.elementsOnPage
            }
        }

        usersService.searchByText(model)
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

    onClearSearch = (e) => {

        if (e.target.value == "") {

            this.setState({
                searchText: ""
            });

            var model =
            {
                "pageNumber": this.state.currentPage,
                "elementsOnPage": this.state.elementsOnPage
            }

            this.getStudents(model);
        }
    }

    render() {

        const columns = [
            {
                key: '1',
                title: 'Name',
                dataIndex: 'name',
                sorter: (a, b) => a.name.localeCompare(b.name)
            },
            {
                key: '2',
                title: 'Surname',
                dataIndex: 'surname',
                sorter: (a, b) => a.name.localeCompare(b.name)
            },
            {
                key: '3',
                title: 'Email',
                dataIndex: 'email',
                sorter: (a, b) => a.name.localeCompare(b.name)
            },
            {
                key: '4',
                title: 'Age',
                dataIndex: 'age',
                sorter: (a, b) => a.age - b.age
            },
            {
                key: '5',
                dataIndex: 'isEmailConfirmed',
                title: 'Is email confirmed',
                filters: [
                    {
                        text: 'True',
                        value: 'true',
                    },
                    {
                        text: 'False',
                        value: 'false',
                    },
                ],
                onFilter: (value, record) => record.isEmailConfirmed.indexOf(value) === 0
            },
            {
                key: '6',
                title: 'Subs. count',
                dataIndex: 'subscriptionsCount',
                sorter: (a, b) => a.subscriptionsCount - b.subscriptionsCount
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
                    onChange={this.onClearSearch}
                    onSearch={this.onSearch}
                    style={{ "marginBottom": "20px" }}
                />

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