import * as React from 'react';
import { NavBar } from './NavBar';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class Layout extends React.Component<LayoutProps, {}> {
    public render() {
        return <div>
            <NavBar />
            <div className='container-fluid'>
                <div className='row'>
                    <div className='col-sm-12'>
                        {this.props.children}
                    </div>
                </div>
            </div>
        </div>;
    }
}
