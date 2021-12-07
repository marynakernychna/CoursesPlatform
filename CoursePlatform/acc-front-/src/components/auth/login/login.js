import styles from '../login/styles.module.css'
import React, { Fragment } from 'react';
import { Input, Button, Form } from "antd";
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import FacebookLogin from 'react-facebook-login';
import authService from '../service';
import { roles } from '../../../actions/account/roles';
import EclipseWidget from '../../eclipse';
import { alertTypes } from '../../alert/types';
import Alerts from '../../alert/index';
// import FacebookLogin from 'react-facebook-login';

class Login extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            role: this.props.role,
            isAlert: this.props.isAlert,
            loading: this.props.loading
        };
    }

    componentDidMount() {
        document.title = 'Log in';
    };

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            role: nextProps.role,
            isAlert: nextProps.isAlert,
            loading: nextProps.loading
        }
    }

    logIn = (userData) => {

        const {
            startLoading,
            finishLoading,
            setAccess
        } = this.props;

        startLoading();

        authService.logIn(userData)
            .then((response) => {

                setAccess(response.data.jwtToken);

                this.redirect(this.state.role);
            },
                err => {
                    this.setWarning(err.response.status);
                })
            .catch(err => {
                console.log("Frontend error", err);
                finishLoading();
            });
    }

    redirect = (role) => {
        const {
            finishLoading
        } = this.props;

        finishLoading();

        switch (role) {
            case roles.STUDENT:
                this.props.history.push("/studentPanel");
                break;
            case roles.ADMIN:
                this.props.history.push("/adminPanel");
                break;
        }
    }

    setWarning = (err) => {
        const {
            setAlert,
            finishLoading
        } = this.props;

        var message;
        switch (err) {
            case 400: {
                message = "Uncorrect login or password !";
                break;
            }
            case 401: {
                message = "Confirm your email !";
                break;
            }
            default: {
                message = "Server error. Try again !";
            }
        }

        var model = {
            type: alertTypes.WARNING,
            message: message
        }

        setAlert(model);
        finishLoading();
    }

    responseFacebook = (response) => {
        // var userData = {
        //     name: response.first_name,
        //     surname: response.last_name,
        //     email: response.email
        // }

        console.log(response);
    }

    render() {

        const { loading, isAlert } = this.state;

        return (
            <Fragment>
                <div className={styles.loginBlock}>

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
                                    message: 'Input your password!',
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
                        fields="first_name, last_name, picture, email"
                        callback={this.responseFacebook}
                        cssClass="ant-btn ant-btn-primary login-form-button"
                        buttonStyle={{ width: '100%' }}
                    />

                    <div className={styles.loginBottomText}>
                        Or <a href="/registration"
                            style={{ fontStyle: 'italic', fontWeight: '500' }}>
                            register now!</a>
                    </div>

                </div>

                {isAlert && <Alerts />}

                {loading && <EclipseWidget />}

            </Fragment >
        )
    };
}

export default Login;