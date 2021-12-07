import styles from '../registration/styles.module.css';
import { Form, Input, Button, InputNumber } from 'antd';
import React, { Fragment } from 'react';
import authService from '../service';
import EclipseWidget from '../../eclipse'
import { alertTypes } from '../../alert/types';
import Alerts from '../../alert/index';

class Registration extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: this.props.loading,
            isAlert: this.props.isAlert
        };
    }

    componentDidMount() {
        document.title = 'Registration';
    };

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            loading: nextProps.loading,
            isAlert: nextProps.isAlert
        }
    }

    register = (userData) => {

        const {
            startLoading,
            finishLoading
        } = this.props;

        startLoading();

        authService.registerUser(userData)
            .then(() => {

                this.props.history.push("/login");

                finishLoading();
            },
                err => {
                    this.setWarning(err.response.status);
                })
            .catch(err => {
                console.log("Frontend error", err);
                finishLoading();
            });
    }

    setWarning = (err) => {
        const {
            setAlert,
            finishLoading
        } = this.props;

        var message;
        err === 400 ? message = "An account with this email already exists !" :
            message = "Server error. Try again !";

        var model = {
            type: alertTypes.WARNING,
            message: message
        }

        setAlert(model);
        finishLoading();
    }

    render() {

        const { loading, isAlert } = this.state;

        return (
            <Fragment>
                <div className={styles.registrationBlock}>

                    <h3 style={{ marginBottom: '35px', color: 'white' }}>Registration</h3>

                    <Form
                        name="register"
                        onFinish={this.register}
                        scrollToFirstError
                    >
                        <Form.Item
                            name="email"
                            rules={[
                                {
                                    type: 'email',
                                    message: 'The input is not valid E-mail!',
                                },
                                {
                                    required: true,
                                    message: 'Please input your E-mail!',
                                },
                            ]}
                        >
                            <Input placeholder="Email" />
                        </Form.Item>

                        <Form.Item
                            name="name"
                            rules={[
                                {
                                    type: 'string',
                                    message: 'The input is not valid Name!',
                                },
                                {
                                    required: true,
                                    message: 'Please input your Name!',
                                },
                            ]}
                        >
                            <Input placeholder="Name" />
                        </Form.Item>

                        <Form.Item
                            name="surname"
                            rules={[
                                {
                                    type: 'string',
                                    message: 'The input is not valid Surname!',
                                },
                                {
                                    required: true,
                                    message: 'Please input your Surname!',
                                },
                            ]}
                        >
                            <Input placeholder="Surname" />
                        </Form.Item>

                        <Form.Item
                            name="age"
                            rules={[
                                {
                                    type: 'number',
                                    message: 'The input is not valid Age!',
                                },
                                {
                                    required: true,
                                    message: 'Please input your Age!',
                                },
                            ]}
                        >
                            <InputNumber
                                min={14}
                                max={100}
                                placeholder="Age" />
                        </Form.Item>

                        <Form.Item
                            name="password"
                            rules={[
                                {
                                    required: true,
                                    message: 'Please input your password!',
                                },
                            ]}
                            hasFeedback
                        >
                            <Input.Password placeholder="Password" />
                        </Form.Item>

                        <Form.Item
                            name="confirm"
                            dependencies={['password']}
                            hasFeedback
                            rules={[
                                {
                                    required: true,
                                    message: 'Please confirm your password!',
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
                            <Input.Password placeholder="Confirm password" />
                        </Form.Item>

                        <Form.Item>
                            <Button type="primary"
                                htmlType="submit"
                                className="registerBtn"
                                style={{ width: '100%' }}>
                                Register
                            </Button>

                        </Form.Item>
                    </Form>

                    <div className={styles.registrationBottomText}>
                        Or <a href="/login"
                            style={{ fontStyle: 'italic', fontWeight: '500' }}>
                            login now!</a>
                    </div>
                </div>

                {isAlert && <Alerts />}

                {loading && <EclipseWidget />}

            </Fragment>
        );
    }
}

export default Registration;