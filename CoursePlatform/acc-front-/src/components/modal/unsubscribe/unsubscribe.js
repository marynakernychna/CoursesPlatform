import React from 'react';
import { Modal, Form } from 'antd';
import coursesService from '../../../services/courses';
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

        coursesService.unsubscribe(request)
            .then(() => {

                setAlert({
                    type: alertTypes.INFO,
                    message: 'You have successfully unsubscribe from course ! '
                });

                setIsElementsChanged();
            },
                err => {
                    setAlert({
                        type: alertTypes.WARNING,
                        message: "Server error. Try again !"
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