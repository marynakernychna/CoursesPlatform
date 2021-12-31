import React from 'react';
import { Layout, Card, Button, Form, Input } from 'antd';
import coursesService from '../../../../services/courses';
import { alertTypes } from '../../../alert/types';

const { TextArea } = Input;

class AddCourse extends React.Component {

    constructor(props) {
        super(props);
    }

    addCourse = (model) => {
        const {
            startLoading,
            finishLoading,
            setAlert
        } = this.props;

        startLoading();

        coursesService.addCourse(model)
            .then(() => {

                setAlert({
                    type: alertTypes.INFO,
                    message: "You have successfully added the course !"
                });
            },
                err => {
console.log(err.response);
                    this.setWarningAlert();
                })
            .catch(err => {

                this.setWarningAlert();
            })
            .finally(() => {
                finishLoading();
            });
    };

    setWarningAlert = () => {

        const {
            setAlert
        } = this.props;

        var model = {
            type: alertTypes.WARNING,
            message: "Something went wrong, try again !"
        }

        setAlert(model);
    }

    render() {

        return (

            <Card hoverable
                style={{ margin: '14px 0px' }}
            >
                <Form name="basic"
                    onFinish={this.addCourse}>

                    <Form.Item label="Enter course title :" name="title"
                        rules={[
                            {
                                required: true,
                                message: 'Please enter course title!',
                            },
                        ]}>
                        <TextArea allowClear
                            showCount autoSize maxLength={70} minLength={10}
                        />
                    </Form.Item>

                    <Form.Item label="Enter course description :" name="description"
                        rules={[
                            {
                                required: true,
                                message: 'Please enter course description!',
                            },
                        ]}
                    >
                        <TextArea allowClear
                            showCount autoSize maxLength={150} minLength={15}
                        />
                    </Form.Item>

                    <Form.Item label="Enter image url :" name="imageUrl"
                        rules={[
                            {
                                required: true,
                                message: 'Please enter image url!',
                            },
                        ]}
                    >
                        <TextArea allowClear
                            showCount autoSize minLength={7}
                        />
                    </Form.Item>

                    <Form.Item>
                        <Button type="primary" htmlType="submit">Add</Button>
                    </Form.Item>
                </Form>
            </Card>

        );
    }
}

export default AddCourse;