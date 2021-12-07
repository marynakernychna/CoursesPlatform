import React from 'react';
import { Modal, Form, Button } from 'antd';
import coursesService from '../../../services/courses';
import { DatePicker } from 'antd';
import moment from 'moment';
import { alertTypes } from '../../alert/types';

class Enroll extends React.Component {

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
            },
            dateFormat: "YYYY-MM-DD"
        };
    }

    closeModal = () => {
        const {
            closeModal
        } = this.props;

        closeModal();
    };

    setDisabledDate = (current) => {
        const now = moment();

        return (
            current &&
            (current < now.subtract(1, "day") || current > now.add(1, "years"))
        );
    }

    enroll = (courseDate) => {

        const {
            startLoading,
            finishLoading,
            setAlert
        } = this.props;

        startLoading();

        const request = {
            startDate: moment(courseDate.startDate._d).format('YYYY-MM-DD'),
            courseId: this.props.info.id
        }
        
        this.closeModal();

        coursesService.enroll(request)
            .then(() => {

                setAlert({
                    type: alertTypes.INFO,
                    message: "You have successfully enrolled ! " +
                        "The course is available in the <Subscriptions> section ."
                });
            },
                err => {
                    this.setWarning(err.response.data.status);
                })
            .catch(err => {
                console.log("Frontend error", err);
            })
            .finally(() => {
                finishLoading();
            });
    };

    setWarning = (err) => {

        const {
            setAlert
        } = this.props;

        var message;
        err === 400 ? message = "You are already enrolled !" :
            message = "Server error. Try again !";

        setAlert({
            type: alertTypes.WARNING,
            message: message
        });
    }

    render() {

        return (
            <Modal title="Enroll in the course"
                visible={true}
                onCancel={() => this.closeModal()}
                okButtonProps={{ style: { display: 'none' } }}>

                <Form {... this.state.layout} name="basic"
                    onFinish={this.enroll}
                >

                    <Form.Item label="Choose start date :"
                        name="startDate"
                        rules={[
                            {
                                required: true,
                                message: 'Please choose start date!',
                            },
                        ]}
                    >
                        <DatePicker format={this.state.dateFormat}
                            disabledDate={this.setDisabledDate}
                        />
                    </Form.Item>

                    <Form.Item {... this.state.tailLayout}>
                        <Button type="primary" htmlType="submit">
                            Submit</Button>
                    </Form.Item>
                </Form>

            </Modal>
        );
    }
}

export default Enroll;