import * as React from 'react';
import { RatingsTable } from './RatingsTable';
import { LastUpdateText } from '../Common/LastUpdateText';

interface RatingsProps {
    headerSize?: number;
}

interface RatingsState {
    lastUpdate: Date;
}

export class Ratings extends React.Component<RatingsProps, RatingsState> {
    constructor(props: RatingsProps) {
        super(props);

        this.state = { lastUpdate: new Date() };

        this.onRatingsUpdated = this.onRatingsUpdated.bind(this);
    }

    public render() {
        return <div>
            {this.getHeader()}
            <LastUpdateText timestamp={this.state.lastUpdate} />
            <RatingsTable onRatingsUpdate={this.onRatingsUpdated} />
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

    onRatingsUpdated() {
        this.setState({ lastUpdate: new Date() });
    }
}
