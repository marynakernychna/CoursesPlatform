import React from 'react';
import styles from '../styles.module.css';
import { Input, Button, Form } from "antd";
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import FacebookLogin from 'react-facebook-login';
import { roles } from '../../../constants/roles';
import { alertTypes } from '../../alert/types';
import authService from '../../../services/auth';
import { pagesNames } from '../../../constants/pagesNames';

class Login extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            role: this.props.role
        };
    }

    componentDidMount() {
        document.title = 'Log in';
    };

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            role: nextProps.role
        }
    }

    logIn = (userData) => {

        const {
            startLoading,
            finishLoading,
            setAccess,
            setAlert
        } = this.props;

        startLoading();

        authService.logIn(userData)
            .then((response) => {

                setAccess(response.data);
                this.redirect(this.state.role);
            },
                err => {

                    this.setWarning(err.response.data);
                })
            .catch(() => {

                setAlert({ type: alertTypes.WARNING, message: "Something went wrong. Try again !" });
            })
            .finally(() => {

                finishLoading();
            })
    }

    responseFacebook = (response) => {

        const {
            startLoading,
            finishLoading,
            setAccess,
            setAlert
        } = this.props;

        startLoading();

        var model = {
            value: response.accessToken
        }

        authService.LogInViaFacebook(model)
            .then((response_) => {

                setAccess(response_.data);
                this.redirect(this.state.role);
            },
                err => {

                    setAlert({ type: alertTypes.WARNING, message: "Something went wrong. Try again !" });
                })
            .catch(() => {

                setAlert({ type: alertTypes.WARNING, message: "Something went wrong. Try again !" });
            })
            .finally(() => {

                finishLoading();
            })
    }

    redirect = (role) => {
        const {
            setAlert,
            finishLoading
        } = this.props;

        finishLoading();

        switch (role) {
            case roles.STUDENT:
                {
                    this.props.history.push("/student/courses");
                    break;
                }
            case roles.ADMINISTRATOR:
                {
                    this.props.history.push("/admin/courses");
                    break;
                }
            default: {
                setAlert({
                    type: alertTypes.WARNING,
                    message: "Something went wrong. Try again !"
                });
            }
        }
    }

    setWarning = (data) => {

        const {
            setAlert,
            finishLoading
        } = this.props;

        var model = {
            type: alertTypes.WARNING,
            message: data.errors.Message != undefined ?
                data.errors.Message : "Uncorrect login or password. Try again !"
        }

        setAlert(model);
        finishLoading();
    }

    changePage = () => {
        const {
            changeAuthPage
        } = this.props;

        changeAuthPage(pagesNames.REGISTRATION);
    }

    render() {

        return (
            <>
                <h3 style={{ marginBottom: '35px', color: 'white' }}>Log in</h3>

                <Form
                    name="basic"
                    initialValues={{
                        remember: true,
                    }}
                    onFinish={this.logIn}
                >
                    <Form.Item
                        name="email"
                        rules={[
                            {
                                required: true,
                                message: 'Input your email!',
                                type: 'email'
                            },
                        ]}
                    >
                        <Input
                            prefix={<UserOutlined />}
                            className="site-form-item-icon"
                            placeholder="email" />
                    </Form.Item>

                    <Form.Item
                        name="password"
                        rules={[
                            {
                                required: true,
                                message: 'Input your password!'
                            },
                        ]}
                    >

                        <Input.Password
                            placeholder="password"
                            prefix={<LockOutlined />}
                            className="site-form-item-icon" />
                    </Form.Item>

                    <Form.Item>
                        <Button type="primary"
                            htmlType="submit"
                            style={{ width: '100%' }}
                        >
                            Log in
                        </Button>
                        <br />
                    </Form.Item>
                </Form>

                <FacebookLogin
                    appId="651731139077984"
                    autoLoad={false}
                    callback={this.responseFacebook}
                    cssClass="ant-btn ant-btn-primary login-form-button"
                    buttonStyle={{ width: '100%' }}
                />

                <div className={styles.bottomText}>
                    Or <a
                        style={{ fontStyle: 'italic', fontWeight: '500' }}
                        onClick={() => this.changePage()}>
                        register now!</a>
                </div>
            </>
        )
    };
}

export default Login;