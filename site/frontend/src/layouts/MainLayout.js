import React from 'react'
import Header from '../components/Header';

class MainLayout extends React.Component {
    render() {
        return (
            <>
                <Header></Header>
                <main>{this.props.children}</main>
            </>
        );
    }
}

export default MainLayout;