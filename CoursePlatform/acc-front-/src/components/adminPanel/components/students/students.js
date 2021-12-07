import React from 'react';
import { Layout, Button, Space, Table } from 'antd';
import { FormOutlined, CloseOutlined, DatabaseFilled } from '@ant-design/icons';
import coursesService from '../../../studentPanel/service';
import EclipseWidget from '../../../eclipse'
import Alerts from "./components/alerts/index"
import Modals from './components/modals/index';
import * as modalTypes from "../../../adminPanel/components/students/components/modals/modalTypes"

const { Content } = Layout;

class Students extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            students: [],
            isModalOpen: this.props.isModalOpen,
            loading: this.props.loading,
            isAlert: this.props.isAlert
        };
    }

    componentDidMount() {

        this.getStudents();
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            loading: nextProps.loading,
            isModalOpen: nextProps.isModalOpen,
            isAlert: nextProps.isAlert,
            students: nextProps.students
        }
    }

    // students actions

    getStudents() {

        const {
            getStudentsStarted,
            getStudentsSuccess,
            getStudentsFailed
        } = this.props;

        getStudentsStarted();

        coursesService.getStudents()
            .then((response) => {

                response.data.map((info, index) =>
                {
                    info.isEmailConfirmed = info.isEmailConfirmed.toString();

                    info.subscriptions.map((sub, index) =>
                    {
                        sub.id = index + 1;
                    })
                })

                getStudentsSuccess(response.data);
            },
                err => {
                    getStudentsFailed();
                })
            .catch(err => {
                console.log("Frontend error", err);
            });
    }

    // modals actions

    openModal = (student, type) => {
        const {
            showModal
        } = this.props;

        showModal({ type: type, data: student })
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
                        {/* <Button type="primary" icon={<DatabaseFilled />}
                            className="edit-form-button"
                            onClick={() => this.openModal(record, modalTypes.VIEWSUBSCRIPTIONS)}>
                            Show subscriptions
                        </Button> */}
                        <Button type="primary" icon={<FormOutlined />}
                            className="edit-form-button"
                            style={{ backgroundColor: "orange" }}
                            onClick={() => this.openModal(record, modalTypes.EDIT)}>
                            Edit
                        </Button>
                        <Button type="primary" icon={<CloseOutlined />}
                            className="edit-form-button" danger
                            onClick={() => this.openModal(record, modalTypes.REMOVE)}>
                            Remove
                        </Button>
                    </Space>
            }
        ];

        const coursesColumns= [
            {
                title: 'Id',
                dataIndex: 'id',
            },
            {
                title: 'Title',
                dataIndex: 'title',
            },
            {
                title: 'Description',
                dataIndex: 'description',
            }
        ]

        const tableProps = {
            expandedRowRender: record => (console.log(record.subscriptions),
            <Table columns={coursesColumns} dataSource={record.subscriptions} pagination={false}/>
            ),
            rowExpandable: record => record.subscriptionsCount > 0
          };

        const { loading, isModalOpen, isAlert } = this.state;

        return (

            <Layout className="site-layout">

                {isAlert && <Alerts />}

                <Content
                    className="site-layout-background"
                    style={{
                        padding: 30,
                    }}
                >

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

                </Content>

                {isModalOpen && <Modals />}

                {loading && <EclipseWidget />}

            </Layout>
        );
    }
}

export default Students;