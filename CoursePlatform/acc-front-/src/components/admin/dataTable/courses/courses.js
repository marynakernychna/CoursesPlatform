import React from 'react';
import { Layout, Button, Space, Table } from 'antd';
import { FormOutlined, CloseOutlined } from '@ant-design/icons';
import coursesService from '../../../../services/courses';
import { modalsTypes } from '../../../modal/modalsTypes';
import { alertTypes } from '../../../alert/types';
import moment from 'moment';

const { Content } = Layout;

class Courses extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            courses: [],

            dateFormat: "YYYY-MM-DD"
        };
    }

    componentDidMount() {

        document.title = "Courses";

        this.getCourses();
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {
        
            return {
                courses: nextProps.courses
            }
    }

    getCourses() {

        const {
            startLoading,
            finishLoading,
            setCourses
        } = this.props;

        startLoading();

        var model = {
            pageNumber: 1,
            elemOnPage: 20,
            sortDirection: 0,
            sortBy: 0
        }

        coursesService.getAllCourses(model)
            .then((response) => {

                var key = 0;
                response.data.courses.map((info, index) => {
                    info.key = key;
                    info.createDate = moment(info.createDate).format(this.state.dateFormat);
                    key = key + 1;
                })

                setCourses(response.data.courses);
            },
                err => {
                    this.setWarningAlert();
                })
            .catch(err => {
                console.log("Frontend error", err);
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

    openModal = (record) => {
        const {
            openModal
        } = this.props;

        openModal({ type: modalsTypes.EDIT_COURSE, info: record });
    }

    render() {

        const columns = [
            {
                key: '1',
                title: 'Title',
                dataIndex: 'title',
                sorter: (a, b) => a.title.localeCompare(b.title)
            },
            {
                key: '2',
                title: 'Creation date',
                dataIndex: 'createDate',
                // filters: [
                //     {
                //         text: 'True',
                //         value: 'true',
                //     },
                //     {
                //         text: 'False',
                //         value: 'false',
                //     },
                // ],
                // onFilter: (value, record) => record.isEmailConfirmed.indexOf(value) === 0
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
                            onClick={() => this.openModal(record)}>
                            Edit
                        </Button>
                        <Button type="primary" icon={<CloseOutlined />}
                            className="edit-form-button" danger
                            onClick={() => this.openModal(record)}>
                            Remove
                        </Button>
                    </Space>
            }
        ];

        const tableProps = {
            expandedRowRender: record => <p style={{ margin: 0 }}>{record.description}</p>
        };

        return (

            <Content
                style={{
                    padding: 30,
                    minHeight: '100vh !importamt'
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
                    dataSource={this.state.courses}
                />

            </Content>
        );
    }
}

export default Courses;