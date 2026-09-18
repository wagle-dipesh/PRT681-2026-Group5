"use client";

import { useState } from "react";
import {
  Grid,
  GridColumn,
  GridDataStateChangeEvent,
} from "@progress/kendo-react-grid";
import { process, State } from "@progress/kendo-data-query";
import { Task } from "../types/task";


interface TaskGridProps {
  initialTasks: Task[];
}

export default function TaskGrid({ initialTasks }: TaskGridProps) {
  const [tasks] = useState<Task[]>(initialTasks);

  const [dataState, setDataState] = useState<State>({
    skip: 0,
    take: 5,
    sort: [],
    filter: undefined,
  });

  const handleDataStateChange = (
    event: GridDataStateChangeEvent
  ) => {
    setDataState(event.dataState);
  };

  return (
    <section>
      <div className="mb-4">
        <h2 className="text-2xl font-semibold">Task Management</h2>

        <p className="text-gray-600">
          Search, sort and manage application tasks.
        </p>
      </div>

      <div className="overflow-x-auto rounded-lg bg-white shadow">
        <Grid
          data={process(tasks, dataState)}
          dataItemKey="id"
          {...dataState}
          pageable
          sortable
          filterable
          onDataStateChange={handleDataStateChange}
          style={{ minWidth: "800px" }}
        >
          <GridColumn field="id" title="ID" width="80px" />

          <GridColumn
            field="title"
            title="Task Title"
            minResizableWidth={180}
          />

          <GridColumn
            field="description"
            title="Description"
            minResizableWidth={250}
          />

          <GridColumn
            field="isCompleted"
            title="Completed"
            filter="boolean"
            width="150px"
          />

          <GridColumn
            field="createdAt"
            title="Created"
            width="220px"
          />
        </Grid>
      </div>
    </section>
  );
}