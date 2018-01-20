import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Ratings } from './Ratings';

export class RatingsPage extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <Ratings />;
    }
}
