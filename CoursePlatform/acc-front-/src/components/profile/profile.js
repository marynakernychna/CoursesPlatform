import React from 'react';
import { Card } from 'antd';

const { Meta } = Card;

class Profile extends React.Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        document.title = "Profile";
    }

    loadProfile = () => {

    }

    render() {

        return (

            <div style={{
                width: '100%'
            }} >

                <Card
                    hoverable
                    style={{ margin: '14px' }}
                >
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    <img src='https://i.pinimg.com/564x/e6/4b/9d/e64b9dad0af5d732294e828cbf3b63d2.jpg'
                                        alt="Image not found"
                                        style={{ maxWidth: '150px', maxHeight: '150px' }}
                                    />
                                </td>
                                <td style={{ width: '100%', paddingLeft: "20px" }}>
                                    <h2> Maryna Kernychna </h2>
                                    <Meta description="kernychnamaryna@gmail.com" />
                                    <br />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </Card>

            </div>

        )
    };
}

export default Profile;