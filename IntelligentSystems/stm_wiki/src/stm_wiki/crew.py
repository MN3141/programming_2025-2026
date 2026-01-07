import os
import requests
import datetime as dt
from bs4 import BeautifulSoup
from crewai import Agent, Crew, Process, Task
from crewai.project import CrewBase, agent, crew, task
from crewai.agents.agent_builder.base_agent import BaseAgent
from crewai_tools import SerperDevTool, ScrapeWebsiteTool
from typing import List
# If you want to run a snippet of code before or after the crew starts,
# you can use the @before_kickoff and @after_kickoff decorators
# https://docs.crewai.com/concepts/crews#example-crew-class-with-decorators

@CrewBase
class StmWiki():
    """StmWiki crew"""

    agents: List[BaseAgent]
    tasks: List[Task]

    # Learn more about YAML configuration files here:
    # Agents: https://docs.crewai.com/concepts/agents#yaml-configuration-recommended
    # Tasks: https://docs.crewai.com/concepts/tasks#yaml-configuration-recommended

    # If you would like to add tools to your agents, you can learn more about it here:
    # https://docs.crewai.com/concepts/agents#agent-tools

    _script_dir = os.path.dirname(os.path.abspath(__file__))
    _local_doc_path = os.path.join(_script_dir,"..","..","knowledge", "stm32f4")

    def _check_timestamp(self, TaskOutput):
        local_doc_time_stamp = os.stat(self._local_doc_path).st_mtime
        local_doc_time_stamp = dt.datetime.fromtimestamp(local_doc_time_stamp)

        url = "https://libopencm3.org/docs/latest/stm32f4/html/index.html"
        re = requests.get(url)

        soup = BeautifulSoup(re.text, "html.parser")

        date_text = soup.find(string="Date")
        if date_text:
            date_value = date_text.find_next().get_text(strip=True)
            online_date = dt.datetime.strptime(date_value, "%d %B %Y")
        else:
            print("Warning: Date not found on page!")
            return

        if local_doc_time_stamp < online_date:
            print("Warning: Local documentation seems to be out of date.",
                "Consider syncing it with the online version.")

    @agent
    def doc_checker(self) -> Agent:
        return Agent(
            config=self.agents_config['doc_checker'], # type: ignore[index]
            verbose=True,
            tools=[SerperDevTool(),ScrapeWebsiteTool()]
        )

    @agent
    def programmer(self) -> Agent:
        return Agent(
            config=self.agents_config['programmer'], # type: ignore[index]
            verbose=True
        )

    @agent
    def code_reviewer(self) -> Agent:
        return Agent(
            config=self.agents_config['code_reviewer'], # type: ignore[index]
            verbose=True
        )

    @agent
    def architect(self) -> Agent:
        return Agent(
            config=self.agents_config['architect'], # type: ignore[index]
            verbose=True
        )

    # To learn more about structured task outputs,
    # task dependencies, and task callbacks, check out the documentation:
    # https://docs.crewai.com/concepts/tasks#overview-of-a-task
    @task
    def local_doc_validation(self) -> Task:
        return Task(
            config=self.tasks_config['local_doc_validation'], # type: ignore[index]
            callback = self._check_timestamp
        )

    @task
    def query_search(self) -> Task:
        return Task(
            config=self.tasks_config['query_search'], # type: ignore[index]
        )

    @task
    def snippet_generation(self) -> Task:
        return Task(
            config=self.tasks_config['snippet_generation'], # type: ignore[index]
        )

    @task
    def code_review(self) -> Task:
        return Task(
            config=self.tasks_config['code_review'], # type: ignore[index]
        )

    @task
    def make_report(self) -> Task:
        return Task(
            config=self.tasks_config['make_report'], # type: ignore[index]
            output_file='result.md'
        )

    @crew
    def crew(self) -> Crew:
        """Creates the StmWiki crew"""
        # To learn how to add knowledge sources to your crew, check out the documentation:
        # https://docs.crewai.com/concepts/knowledge#what-is-knowledge

        return Crew(
            agents=self.agents, # Automatically created by the @agent decorator
            tasks=self.tasks, # Automatically created by the @task decorator
            process=Process.sequential,
            verbose=True,
            # process=Process.hierarchical, # In case you wanna use that instead https://docs.crewai.com/how-to/Hierarchical/
        )
