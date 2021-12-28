import React from 'react';
import { Modal, Form, Button, Input, DatePicker } from 'antd';
import usersService from '../../../services/students';
import { alertTypes } from '../../alert/types';
import moment from 'moment';
import { loginViaFacebook } from '../../../constants/others';

const { TextArea } = Input;

class ChangePassword extends React.Component {

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

    closeModal = () => {

        const {
            closeModal
        } = this.props;

        closeModal();
    };

    changePassword = (values) => {

        console.log(values);
        const {
            startLoading,
            finishLoading,
            setAlert,
        } = this.props;

        startLoading();

        if (values.old_password === values.password) {

            this.closeModal();

            setAlert({
                type: alertTypes.INFO,
                message: "You have successfully changed your password!"
            });

            finishLoading();

            return;
        }

        var request = {
                oldPassword: values.old_password,
                newPassword: values.password
            }

        this.closeModal();

        usersService.changePassword(request)
            .then(() => {

                setAlert({
                    type: alertTypes.INFO,
                    message: "You have successfully changed your password!"
                });
            },
                err => {

                    this.setWarningAlert(err.response);
                })
            .catch(err => {
                
                this.setWarningAlert();
            })
            .finally(() => {
                finishLoading();
            });
    };

    setWarningAlert = (err) => {

        const {
            setAlert
        } = this.props;

        var model = {
            type: alertTypes.WARNING,
            message: err != undefined &&
                err.data.errors != undefined &&
                err.data.errors.Message != undefined ?
                err.data.errors.Message :
                "Something went wrong. Try again!"
        }

        setAlert(model);
    }

    render() {

        return (
            <Modal title="Change password"
                visible={true}
                onCancel={() => this.closeModal()}
                afterClose={() => this.closeModal()}
                okButtonProps={{ style: { display: 'none' } }}>

                <Form {... this.state.layout} name="basic"
                    onFinish={(values) => this.changePassword(values)}
                    style={{ "textAlign": 'right' }}
                >
                    <Form.Item
                        name="old_password"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your current password!',
                            },
                        ]}
                        hasFeedback
                    >
                        <Input.Password placeholder="Old password" />
                    </Form.Item>

                    <Form.Item
                        name="password"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your new password!',
                            },
                        ]}
                        hasFeedback
                    >
                        <Input.Password placeholder="New password" />
                    </Form.Item>

                    <Form.Item
                        name="confirm"
                        dependencies={['password']}
                        hasFeedback
                        rules={[
                            {
                                required: true,
                                message: 'Please confirm your new password!',
                            },
                            ({ getFieldValue }) => ({
                                validator(_, value) {
                                    if (!value || getFieldValue('password') === value) {
                                        return Promise.resolve();
                                    }

                                    return Promise.reject(
                                        new Error
                                            (
                                                'The two passwords that you entered do not match!'
                                            )
                                    );
                                },
                            }),
                        ]}
                    >
                        <Input.Password placeholder="Confirm new password" />
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

export default ChangePassword;