import React from 'react';
import { Modal, Form } from 'antd';
import coursesService from '../../../services/courses';
import authService from '../../../services/auth';
import { alertTypes } from '../../alert/types';

class Remove extends React.Component {

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

    remove = () => {

        const {
            startLoading,
            finishLoading,
            setAlert,
            removeCourse
        } = this.props;

        startLoading();

        var request = {
            value: this.props.info.id
        }

        this.closeModal();

        coursesService.removeCourse(request)
            .then(() => {

                setAlert({
                    type: alertTypes.INFO,
                    message: 'You have successfully removed the course ! '
                });

                removeCourse(this.props.info);
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
            message: "Something went wrong. Try again!"
        }

        setAlert(model);
    }

    render() {

        return (
            <Modal title="Removing confirmation"
                visible={true}
                onCancel={() => this.closeModal()}
                afterClose={() => this.closeModal()}
                onOk={() => this.remove()}>

                <Form {... this.state.layout} name="basic"
                    style={{ "textAlign": 'center' }}>

                    <h4>Remove the "{this.props.info.title}" course?</h4>

                </Form>
            </Modal>
        );
    }
}

export default Remove;