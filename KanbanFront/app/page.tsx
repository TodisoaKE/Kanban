"use client"

import { useState, useEffect } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Badge } from "@/components/ui/badge"
import { Plus, CheckCircle, Play } from "lucide-react"
import { useToast } from "@/hooks/use-toast"
import { Textarea } from "@/components/ui/textarea"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"

interface User {
  id: number
  name: string
}

interface Task {
  id: number
  title: string
  description?: string
  status: string
  assignedTo?: string
  createdAt: string
  updatedAt: string
}

let CURRENT_USER_ID = 1 // Simulé pour le test
const baseUrl = "http://localhost:5094"

export default function KanbanBoard() {
  const [tasks, setTasks] = useState<Task[]>([])
  const [newTaskTitle, setNewTaskTitle] = useState("")
  const [loading, setLoading] = useState(false)
  const { toast } = useToast()

  const [newTaskDescription, setNewTaskDescription] = useState("")
  const [selectedUserId, setSelectedUserId] = useState<number | undefined>()
  const [users, setUsers] = useState<User[]>([])

  // Charger les tâches au démarrage
  useEffect(() => {
    fetchTasks()
    fetchUsers()
  }, [])

  const fetchTasks = async () => {
    try {
      const response = await fetch(baseUrl+"/api/tasks")
      const data = await response.json()
      console.log(data)
      const mappedTasks = data.data.map((task: Task) => ({
        ...task
      }))
      setTasks(mappedTasks)
    } catch (error) {
      toast({
        title: "Erreur",
        description: "Impossible de charger les tâches",
        variant: "destructive",
      })
    }
  }

  const fetchUsers = async () => {
    try {
      const response = await fetch(baseUrl+"/api/User")
      const data = await response.json()
      const mappedUsers = data.data.map((user: User) => ({ id: user.id, name: user.name }))
      CURRENT_USER_ID = mappedUsers[0].id
      setUsers(mappedUsers)
    } catch (error) {
      console.error("Erreur lors du chargement des utilisateurs:", error)
    }
  }

  const createTask = async () => {
    if (!newTaskTitle.trim()) return

    setLoading(true)
    try {
      const response = await fetch(baseUrl+"/api/tasks", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          title: newTaskTitle,
          description: newTaskDescription.trim() || undefined,
          assignedToUserId: selectedUserId || undefined,
          createdByUserId: CURRENT_USER_ID,
        }),
      })

      if (response.ok) {
        const newTask = await response.json()
        setTasks((prev) => [...prev, newTask])
        setNewTaskTitle("")
        setNewTaskDescription("")
        setSelectedUserId(undefined)
        toast({
          title: "Succès",
          description: "Tâche créée avec succès",
        })
        fetchTasks()
      }
    } catch (error) {
      toast({
        title: "Erreur",
        description: "Impossible de créer la tâche",
        variant: "destructive",
      })
    } finally {
      setLoading(false)
    }
  }

  const updateTaskStatus = async (taskId: number, newStatus: string) => {
    try {
      const response = await fetch(baseUrl+`/api/tasks/${taskId}/status`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          newStatus,
          changedByUserId: CURRENT_USER_ID,
        }),
      })

      if (response.ok) {
        const updatedTask = await response.json()
        setTasks((prev) => prev.map((task) => (task.id === taskId ? updatedTask : task)))
        toast({
          title: "Succès",
          description: "Statut mis à jour",
        })
        fetchTasks()
      } else {
        const error = await response.json()
        toast({
          title: "Erreur",
          description: error.message || "Transition invalide",
          variant: "destructive",
        })
      }
    } catch (error) {
      toast({
        title: "Erreur",
        description: "Impossible de mettre à jour le statut",
        variant: "destructive",
      })
    }
  }

  const getTasksByStatus = (status: string) => {
    console.log(tasks)
    return tasks.filter((task) => task.status === status)
  }

  const getNextStatus = (currentStatus: string) => {
    switch (currentStatus) {
      case "ToDo":
        return "InProgress"
      case "InProgress":
        return "Done"
      default:
        return null
    }
  }

  const getStatusButton = (task: Task) => {
    const nextStatus = getNextStatus(task.status)
    if (!nextStatus) return null

    const buttonConfig = {
      InProgress: { label: "Start", icon: Play, variant: "default" as const },
      Done: { label: "Complete", icon: CheckCircle, variant: "default" as const },
    }

    const config = buttonConfig[nextStatus as keyof typeof buttonConfig]
    const Icon = config.icon

    return (
      <Button
        size="sm"
        variant={config.variant}
        onClick={() => updateTaskStatus(task.id, nextStatus)}
        className="w-full mt-2"
      >
        <Icon className="w-4 h-4 mr-1" />
        {config.label}
      </Button>
    )
  }

  const columns = [
    { title: "À Faire", status: "ToDo", color: "bg-slate-100" },
    { title: "En Cours", status: "InProgress", color: "bg-blue-50" },
    { title: "Terminé", status: "Done", color: "bg-green-50" },
  ]

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-7xl mx-auto">
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">Tableau Kanban</h1>

          {/* Formulaire de création de tâche amélioré */}
          <Card className="mb-6">
            <CardHeader>
              <CardTitle className="text-lg">Créer une nouvelle tâche</CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <Input
                placeholder="Titre de la tâche..."
                value={newTaskTitle}
                onChange={(e) => setNewTaskTitle(e.target.value)}
                className="w-full"
              />

              <Textarea
                placeholder="Description de la tâche..."
                value={newTaskDescription}
                onChange={(e) => setNewTaskDescription(e.target.value)}
                className="w-full min-h-[80px] resize-none"
              />

              <Select
                value={selectedUserId?.toString() || "none"}
                onValueChange={(value) => setSelectedUserId(value === "none" ? undefined : Number(value))}
              >
                <SelectTrigger className="w-full">
                  <SelectValue placeholder="Assignation" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">Aucune assignation</SelectItem>
                  {users.map((user) => (
                    <SelectItem key={user.id} value={user.id.toString()}>
                      {user.name}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>

              <Button
                onClick={createTask}
                disabled={loading || !newTaskTitle.trim()}
                className="w-full bg-green-500 hover:bg-green-600 text-white"
              >
                <Plus className="w-4 h-4 mr-2" />
                Ajouter
              </Button>
            </CardContent>
          </Card>
        </div>

        {/* Tableau Kanban */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          {columns.map((column) => (
            <div key={column.status} className={`${column.color} rounded-lg p-4`}>
              <div className="flex items-center justify-between mb-4">
                <h2 className="font-semibold text-lg text-gray-800">{column.title}</h2>
                <Badge variant="secondary">{getTasksByStatus(column.status).length}</Badge>
              </div>

              <div className="space-y-3">
                {getTasksByStatus(column.status).map((task) => (
                  <Card key={task.id} className="shadow-sm hover:shadow-md transition-shadow">
                    <CardContent className="p-4">
                      <h3 className="font-medium text-gray-900 mb-2">{task.title}</h3>

                      {task.description && <p className="text-sm text-gray-600 mb-2">{task.description}</p>}

                      {task.assignedTo && (
                        <div className="flex items-center gap-2 mb-2">
                          <Badge variant="outline" className="text-xs">
                            {task.assignedTo}
                          </Badge>
                        </div>
                      )}

                      <div className="text-xs text-gray-500 mb-2">
                        Créé le {new Date(task.createdAt).toLocaleDateString("fr-FR")}
                      </div>

                      {getStatusButton(task)}
                    </CardContent>
                  </Card>
                ))}

                {getTasksByStatus(column.status).length === 0 && (
                  <div className="text-center py-8 text-gray-500">
                    <p className="text-sm">Aucune tâche</p>
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
