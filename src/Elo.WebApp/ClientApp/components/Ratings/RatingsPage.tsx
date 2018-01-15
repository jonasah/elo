import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { AddGameForm } from './AddGameForm';
import { RatingsTable } from './RatingsTable';

export class RatingsPage extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <h1>Ratings</h1>
            <RatingsTable />
        </div>;
    }
}
