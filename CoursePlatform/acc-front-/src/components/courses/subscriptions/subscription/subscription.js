import React from 'react';
import { Card, Button } from 'antd';
import moment from 'moment';
import { modalsTypes } from '../../../modal/modalsTypes';
import { CloseOutlined } from '@ant-design/icons';

const { Meta } = Card;

class Subscription extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            dateFormat: "YYYY-MM-DD"
        };
    }

    openModal = (info) => {
        const {
            openModal
        } = this.props;

        openModal({ type: modalsTypes.UNSUBSCRIBE, info: info })
    }

    render() {

        return (
            <div style={{ width: '100%' }} >
                <Card
                    hoverable
                    style={{ margin: '14px 0px' }}
                >
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    <img src={this.props.info.imageUrl}
                                        alt="Image not found"
                                        style={{ maxWidth: '150px', maxHeight: '150px' }}
                                    />
                                </td>
                                <td style={{ width: '100%', paddingLeft: "20px" }}>
                                    <h2> {this.props.info.title}</h2>
                                    <h4> {this.props.info.description} </h4>
                                    <br />

                                    <Meta description={moment(this.props.info.startDate).format(this.state.dateFormat)} />
                                    <Button type="primary"
                                        danger
                                        icon={<CloseOutlined />}
                                        style={{ marginTop: '10px', width: '100%' }}
                                        onClick={() => this.openModal(this.props.info)}>
                                        Unsubscribe
                                    </Button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </Card>
            </div>
        );
    }
}

export default Subscription;