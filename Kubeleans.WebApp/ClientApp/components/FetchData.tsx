import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';

interface FetchDataExampleState {
    foodData: FoodDatum[];
    loading: boolean;
}

export class FetchData extends React.Component<RouteComponentProps<{}>, FetchDataExampleState> {
    constructor() {
        super();
        this.state = { foodData: [], loading: true };

        fetch('api/SampleData/FoodData')
            .then(response => response.json() as Promise<FoodDatum[]>)
            .then(data => {
                this.setState({ foodData: data, loading: false });
            });
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.foodData);

        return <div>
            <h1>Food data</h1>
            <p>This component fetches information about your favorite foods.</p>
            { contents }
        </div>;
    }

    private static renderForecastsTable(foodData: FoodDatum[]) {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Output</th>
                </tr>
            </thead>
            <tbody>
            {foodData.map(foodDatum =>
                <tr key={ foodDatum.name }>
                    <td>{ foodDatum.name }</td>
                    <td>{ foodDatum.content }</td>
                </tr>
            )}
            </tbody>
        </table>;
    }
}

interface FoodDatum {
    name: string;
    content: string;
}
