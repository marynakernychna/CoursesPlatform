import React from 'react';
import { Modal, Form } from 'antd';
import usersService from '../../../services/students';
import authService from '../../../services/auth';
import { alertTypes } from '../../alert/types';

class RemoveStudent extends React.Component {

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

    removeStudent = () => {

        const {
            startLoading,
            finishLoading,
            setAlert,
            removeStudent
        } = this.props;

        startLoading();

        var request = {
            value: this.props.info.email
        }

        this.closeModal();

        usersService.removeUser(request)
            .then(() => {

                setAlert({
                    type: alertTypes.INFO,
                    message: 'You have successfully removed the student ! '
                });

                removeStudent(this.props.info);
            },
                err => {
                    setAlert({
                        type: alertTypes.WARNING,
                        message: err.response.data.errors.message
                    });

                })
            .catch(err => {
                console.log("Frontend error", err);
            })
            .finally(() => {
                finishLoading();
            });
    }

    render() {

        return (
            <Modal title="Removing confirmation"
                visible={true}
                onCancel={() => this.closeModal()}
                afterClose={() => this.closeModal()}
                onOk={() => this.removeStudent()}>

                <Form {... this.state.layout} name="basic"
                    style={{ "textAlign": 'center' }}>

                    <h4>Remove the "{this.props.info.name} {this.props.info.surname}" student?</h4>

                </Form>
            </Modal>
        );
    }
}

export default RemoveStudent;