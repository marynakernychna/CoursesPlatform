import React from 'react';
import { Modal, Form, Button, Input, DatePicker } from 'antd';
import usersService from '../../../services/students';
import { alertTypes } from '../../alert/types';
import moment from 'moment';
import { loginViaFacebook } from '../../../constants/others';

const { TextArea } = Input;

class EditProfile extends React.Component {

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
            },
            dateFormat: "YYYY-MM-DD"
        };
    }

    closeModal = () => {

        const {
            closeModal
        } = this.props;

        closeModal();
    };

    editProfile = (newData) => {

        const {
            startLoading,
            finishLoading,
            setAlert,
            logOut,
            setProfileInfo,
            setDate
        } = this.props;

        startLoading();

        if (newData.name === undefined) {
            newData.name = this.props.info.name;
        }
        if (newData.surname === undefined) {
            newData.surname = this.props.info.surname;
        }
        if (newData.email === undefined) {
            newData.email = this.props.info.email;
        }
        if (newData.date === undefined) {
            newData.date = this.props.info.birthday;
        }

        if (newData.name == this.props.info.name &&
            newData.surname == this.props.info.surname &&
            newData.email == this.props.info.email &&
            newData.date == this.props.info.birthday) {

            this.closeModal();

            setAlert({
                type: alertTypes.INFO,
                message: "You have successfully edited the student !"
            });

            finishLoading();

            return;
        }

        var request;

        if (newData.date !== loginViaFacebook) {
            request = {
                name: newData.name,
                surname: newData.surname,
                email: newData.email,
                birthday: newData.date,
                currentEmail: this.props.info.email
            }
        }
        else {
            request = {
                name: newData.name,
                surname: newData.surname,
                email: newData.email,
                currentEmail: this.props.info.email
            }
        }

        this.closeModal();
        
        usersService.editProfileInfo(request)
            .then(() => {

                if (newData.email != this.props.info.email) {
                    logOut();
                }

                setProfileInfo({
                    name: newData.name,
                    surname: newData.surname,
                    email: newData.email
                });

                if (newData.date != undefined &&
                    newData.date != loginViaFacebook) {
                        
                    setDate({
                        date: newData.date
                    });
                }

                setAlert({
                    type: alertTypes.INFO,
                    message: "You have successfully edited your profile !"
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
    
    setDisabledDate = (current) => {
        const now = moment();

        return (
            current > now.subtract(14, "years")
        );
    }

    render() {

        return (
            <Modal title="Edit profile info"
                visible={true}
                onCancel={() => this.closeModal()}
                afterClose={() => this.closeModal()}
                okButtonProps={{ style: { display: 'none' } }}>

                <Form {... this.state.layout} name="basic"
                    onFinish={(values) => this.editProfile(values)}
                    style={{ "textAlign": 'right' }}
                >

                    <Form.Item label="Enter new name :" name="name">
                        <TextArea className="name" allowClear
                            showCount autoSize maxLength={20}
                            defaultValue={this.props.info.name}
                            value={this.props.info.name}
                        />
                    </Form.Item>

                    <Form.Item label="Enter new surname :" name="surname">
                        <TextArea className="surname" allowClear
                            showCount autoSize maxLength={50}
                            defaultValue={this.props.info.surname}
                            value={this.props.info.surname}
                        />
                    </Form.Item>

                    <Form.Item label="Enter new email :" name="email">
                        <TextArea className="email"
                            showCount autoSize maxLength={50}
                            defaultValue={this.props.info.email}
                            value={this.props.info.email}
                        />
                    </Form.Item>

                    <Form.Item
                        label="Enter new birthday :" name="date"
                    >
                        <DatePicker format={this.state.dateFormat}
                            disabledDate={this.setDisabledDate}
                            placeholder="Your birhday" />

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

export default EditProfile;