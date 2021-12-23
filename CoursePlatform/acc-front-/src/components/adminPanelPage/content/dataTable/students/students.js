import React from 'react';
import {  Button, Space, Table } from 'antd';
import { FormOutlined, CloseOutlined } from '@ant-design/icons';
import usersService from '../../../../../services/students';
import { modalsTypes } from '../../../../modal/modalsTypes';
import { alertTypes } from '../../../../alert/types';
import moment from 'moment';

class Students extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            students: [],
            dateFormat: "YYYY-MM-DD"
        };
    }

    componentDidMount() {

        document.title = "Students";

        this.getStudents();
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            students: nextProps.data
        }
    }

    getStudents() {

        const {
            startLoading,
            finishLoading,
            setStudents
        } = this.props;

        startLoading();

        usersService.getStudents()
            .then((response) => {

                var studentKey = 0;
                
                response.data.map((info, index) => {
                    
                    info.key = studentKey;
                    info.isEmailConfirmed = info.isEmailConfirmed.toString();
                    info.age = parseInt(moment.duration(moment().diff(new Date(info.birthday))).asYears(), 10);

                    if (info.age < 14) {
                        info.age = "———"
                    };

                    var subscriptionKey = 1;

                    info.subscriptions.map((course, index) => {
                    
                        course.key = subscriptionKey;
                        subscriptionKey = subscriptionKey + 1;
                    })

                    studentKey = studentKey + 1;
                })

                setStudents(response.data);
            },
                err => {
                    console.log(err);
                    this.setWarningAlert();
                })
            .catch(err => {
                
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

            <Table
                {...tableProps}
                columns={columns}
                pagination={{
                    position: ['topCenter', 'bottomCenter'],
                    defaultPageSize: 10,
                    showSizeChanger: true,
                    pageSizeOptions: ['10', '20', '30'],
                }}
                dataSource={this.state.students}
            />
        );
    }
}

export default Students;