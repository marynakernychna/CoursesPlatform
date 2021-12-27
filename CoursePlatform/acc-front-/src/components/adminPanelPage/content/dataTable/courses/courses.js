import React from 'react';
import { Layout, Button, Space, Table } from 'antd';
import { FormOutlined, CloseOutlined } from '@ant-design/icons';
import coursesService from '../../../../../services/courses';
import { modalsTypes } from '../../../../modal/modalsTypes';
import { alertTypes } from '../../../../alert/types';
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
            courses: nextProps.data
        }
    }

    getCourses() {

        const {
            startLoading,
            finishLoading,
            setCourses
        } = this.props;

        startLoading();

        coursesService.getCourses()
            .then((response) => {

                var key = 0;
                response.data.map((info, index) => {
                    info.key = key;
                    info.createDate = moment(info.createDate).format(this.state.dateFormat);
                    key = key + 1;
                })

                setCourses(response.data);
            },
                err => {

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
                title: 'Title',
                dataIndex: 'title',
                sorter: (a, b) => a.title.localeCompare(b.title)
            },
            {
                key: '2',
                title: 'Creation date',
                dataIndex: 'createDate'
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
                            onClick={() => this.openModal(record, modalsTypes.EDIT_COURSE)}>
                            Edit
                        </Button>
                        <Button type="primary" icon={<CloseOutlined />}
                            className="edit-form-button" danger
                            onClick={() => this.openModal(record, modalsTypes.REMOVE_COURSE)}>
                            Remove
                        </Button>
                    </Space>
            }
        ];

        const tableProps = {
            expandedRowRender: record => <p style={{ margin: 0 }}>{record.description}</p>
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
                dataSource={this.state.courses}
            />
        );
    }
}

export default Courses;