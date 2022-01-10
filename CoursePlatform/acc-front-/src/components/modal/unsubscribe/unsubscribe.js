import React from 'react';
import { Modal, Form } from 'antd';
import coursesService from '../../../services/courses';
import authService from '../../../services/auth';
import { alertTypes } from '../../alert/types';

class Unsubscribe extends React.Component {

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

    unsubscribe = (info) => {

        const {
            startLoading,
            finishLoading,
            setAlert,
            setIsElementsChanged
        } = this.props;

        startLoading();

        var request = {
            courseId: this.props.info.id
        }

        this.closeModal();
console.log(request, this.props);
        coursesService.unsubscribe(request)
            .then(() => {

                setAlert({
                    type: alertTypes.INFO,
                    message: 'You have successfully unsubscribe from course ! '
                });

                setIsElementsChanged();
            },
                err => {
                    console.log(err.response.data);

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
            <Modal title="Unsubscribe confirmation"
                visible={true}
                onCancel={() => this.closeModal()}
                afterClose={() => this.closeModal()}
                onOk={() => this.unsubscribe()}>

                <Form {... this.state.layout} name="basic"
                    style={{ "textAlign": 'center' }}>

                    <h4>Unsubscribe from "{this.props.info.title}" course?</h4>

                </Form>
            </Modal>
        );
    }
}

export default Unsubscribe;