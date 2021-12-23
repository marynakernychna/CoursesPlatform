import React from 'react';
import authService from '../../services/auth';
import { alertTypes } from '../alert/types';
import { Spin } from "antd";

class Email extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: this.props.loading
        }
    }

    componentDidMount() {

        const {
            startLoading,
            finishLoading,
            setAlert
        } = this.props;

        startLoading();

        var data = window.location.pathname.split('/');
        var tokenParts = data.slice(2, -1);

        var token = tokenParts.join('/');
        var email = data[data.length - 1];

        var model = {
            token: token,
            email: email
        };

        authService.confirmEmail(model)
            .then(() => {

                setAlert({
                    type: alertTypes.INFO,
                    message: 'You have successfully confirmed your email ! '
                });

            },
                err => {

                    this.setWarningAlert();
                })
            .catch(err => {

                this.setWarningAlert();
            })
            .finally(() => {

                this.props.history.push("/auth");
                finishLoading();
            });
    }

    setWarningAlert = () => {

        const {
            setAlert
        } = this.props;

        var model = {
            type: alertTypes.WARNING,
            message: "Something went wrong. Try to confirm your email again !"
        }

        setAlert(model);
    }

    render() {

        const { loading } = this.state;

        return (

            <Spin size="large" spinning={loading}>

                <div style={{
                    "minHeight": "100%"
                }}>
                </div>

            </Spin>
        );
    }
}

export default Email;