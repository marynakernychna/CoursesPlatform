import styles from '../styles.module.css';
import { Form, Input, Button, Alert, DatePicker } from 'antd';
import React from 'react';
import authService from '../../../services/auth';
import moment from 'moment';
import { alertTypes } from '../../alert/types';
import { pagesNames } from '../../../constants/pagesNames';

class Registration extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            dateFormat: "YYYY-MM-DD"
        };
    }

    componentDidMount() {
        document.title = 'Registration';
    };

    register = (userData) => {

        const {
            startLoading,
            finishLoading,
            setAlert
        } = this.props;

        startLoading();

        authService.registerUser(userData)
            .then(() => {

                this.changePage();
                finishLoading();
            },
                err => {
                    
                    this.setWarning(err.response.data.errors);
                })
            .catch(() => {

                setAlert({ type: alertTypes.WARNING, message: "Something went wrong. Try again !" });
            })
            .finally(() => {

                finishLoading();
            })
    }

    setWarning = (err) => {

        const {
            setAlert,
            finishLoading
        } = this.props;

        if (err != undefined) {

            this.setState({
                errors: err.Password.join(' ')
            })
        }

        var model = {
            type: alertTypes.WARNING,
            message: err != undefined && err.Message != undefined ? err.Message : "Something went wrong. Try again !"
        }

        setAlert(model);
        finishLoading();
    }

    changePage = () => {
        const {
            changeAuthPage
        } = this.props;

        changeAuthPage(pagesNames.LOGIN);
    }

    setDisabledDate = (current) => {
        const now = moment();

        return (
            current > now.subtract(14, "years")
        );
    }

    render() {

        const { errors } = this.state;

        return (
            <>
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
                                pattern: new RegExp(/^[a-zA-Z]+$/i),
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
                                pattern: new RegExp(/^[a-zA-Z]+$/i),
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
                        name="birthday"
                        rules={[
                            {
                                required: true,
                                message: 'Please choose your birthday date!',
                            },
                        ]}
                    >
                        <DatePicker format={this.state.dateFormat}
                            disabledDate={this.setDisabledDate}
                            placeholder="Your birhday" />

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

                    {errors ? (
                        <Alert
                            message={errors}
                            type="error"
                            showIcon
                            closable
                            style={{ marginBottom: 20 }}
                        />
                    ) : null}

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

                <div className={styles.bottomText}>
                    Or <a
                        style={{ fontStyle: 'italic', fontWeight: '500' }}
                        onClick={() => this.changePage()}>
                        login now!</a>
                </div>
            </>
        );
    }
}

export default Registration;