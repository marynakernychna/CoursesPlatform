import React from 'react';
import { Modal, Form, Button, Input } from 'antd';
import coursesService from '../../../services/courses';
import { alertTypes } from '../../alert/types';

const { TextArea } = Input;

class Edit extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            layout: {
                labelCol: {
                    span: 8,
                },
                wrapperCol: {
                    span: 16,
                },
            },
            tailLayout: {
                wrapperCol: {
                    offset: 8,
                    span: 16,
                },
            }
        };
    }

    closeModal = (id) => {

        const {
            closeModal
        } = this.props;

        closeModal(id);
    };

    edit = (info) => {

        const {
            startLoading,
            finishLoading,
            setAlert,
            updateCourses
        } = this.props;

        startLoading();

        if (info.title === undefined) {
            info.title = this.props.info.title;
        }
        if (info.description === undefined) {
            info.description = this.props.info.description;
        }
        if (info.imageUrl === undefined) {
            info.imageUrl = this.props.info.imageUrl;
        }

        if (info.title == this.props.info.title &&
            info.description == this.props.info.description &&
            info.imageUrl == this.props.info.imageUrl) {

            this.closeModal();
            setAlert({
                type: alertTypes.INFO,
                message: "You have successfully edited the course !"
            });
            finishLoading();

            return;
        }

        info['id'] = this.props.info.id;

        this.closeModal();

        coursesService.editCourse(info)
            .then(() => {

                updateCourses({ key: this.props.info.key, newData: info });
                setAlert({
                    type: alertTypes.INFO,
                    message: "You have successfully edited the course !"
                });
            },
                err => {
                    setAlert({
                        type: alertTypes.WARNING,
                        message: "Something went wrong. Try again!"
                    });
                })
            .catch(err => {
                console.log("Frontend error", err);
            })
            .finally(() => {
                finishLoading();
            });
    };

    render() {

        return (
            <Modal title="Edit the course"
                visible={true}
                onCancel={() => this.closeModal()}
                afterClose={() => this.closeModal()}
                okButtonProps={{ style: { display: 'none' } }}>

                <Form {... this.state.layout} name="basic"
                    onFinish={(values) => this.edit(values)}
                    style={{ "textAlign": 'right' }}
                >

                    <Form.Item label="Enter new title :" name="title">
                        <TextArea className="title" allowClear
                            showCount autoSize maxLength={70}
                            defaultValue={this.props.info.title}
                            value={this.props.info.title}
                        />
                    </Form.Item>

                    <Form.Item label="Enter new description :" name="description">
                        <TextArea className="description" allowClear
                            showCount autoSize maxLength={150}
                            defaultValue={this.props.info.description}
                            value={this.props.info.description}
                        />
                    </Form.Item>

                    <Form.Item label="Enter new image url :" name="imageUrl">
                        <TextArea className="imageUrl"
                            autoSize allowClear
                            defaultValue={this.props.info.imageUrl}
                            value={this.props.info.imageUrl}
                        />
                    </Form.Item>

                    <Form.Item {... this.state.tailLayout}
                        style={{ "marginTop": '10px' }}>
                        <Button type="primary" htmlType="submit">Submit</Button>
                    </Form.Item>

                </Form>

            </Modal>
        );
    }
}

export default Edit;