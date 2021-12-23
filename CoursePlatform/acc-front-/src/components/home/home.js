import React from 'react';
import { Button } from 'antd';
import { Link } from 'react-router-dom';
import "./style.css";

class Home extends React.Component {

    componentDidMount() {
        document.title = "Home";
    }

    render() {

        return (
            <>

                <div className='contentBlock'>

                    <table style={{ "height": "100%" }}>
                        <tbody>
                            <tr>
                                <td style={{ "maxWidth": "33%" }}>
                                    <div style={{ "textAlign": "right" }}>
                                        <h1 style={{ "fontSize": "40px", "fontFamily": "sans-serif", "color": "white", "margin": "0", "height": "100%" }}>
                                            Learn programming online with our courses! <br />From anywhere in the world!
                                        </h1>
                                    </div>
                                </td>
                                <td style={{
                                    "padding": "0 10px"
                                }}>
                                    <div style={{
                                        "height": "350%",
                                        "borderLeft": "6px solid white",
                                        "margin": "0 50px"
                                    }}>
                                    </div>
                                </td>
                                <td style={{ "padding": "0" }}>
                                    <div style={{ "textAlign": "left" }}>
                                        <h1 style={{ "fontSize": "40px", "fontFamily": "sans-serif", "color": "white", "margin": "0", "height": "100%" }}>
                                            Log in and learn for free!
                                        </h1>

                                        <Button type="primary" style={{ "marginTop": "10px", "height": "35px" }}>
                                            <Link to="/auth">
                                                Log in
                                            </Link>
                                        </Button>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                </div>

            </>
        );
    }
}

export default Home;