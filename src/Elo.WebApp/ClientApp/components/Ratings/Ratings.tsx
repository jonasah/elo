import * as React from 'react';
import { RatingsTable } from './RatingsTable';

interface RatingsProps {
    headerSize?: number;
}

export class Ratings extends React.Component<RatingsProps, {}> {
    public render() {
        return <div>
            {this.getHeader()}
            <RatingsTable />
        </div>;
    }

    getHeader() {
        switch (this.props.headerSize) {
            case 2:
                return <h2>Ratings</h2>;
            default:
                return <h1>Ratings</h1>;
        }
    }
}
